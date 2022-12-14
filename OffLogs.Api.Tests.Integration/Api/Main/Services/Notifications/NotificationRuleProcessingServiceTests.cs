using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Notifications;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Notifications
{
    public class NotificationRuleProcessingServiceTests : MyApiIntegrationTest
    {
        private readonly INotificationRuleService _notificationRuleService;
        private readonly INotificationRuleProcessingService _processingService;
        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        
        public NotificationRuleProcessingServiceTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
            _processingService = _factory.Services.GetRequiredService<INotificationRuleProcessingService>();
            _ruleFactory = DataFactory.NotificationRuleFactory();
        }

        [Fact]
        public async Task ShouldProcessWaitingRules()
        {
            await RulesAsExecutingAsync();

            var expectedCount = 2;
            var expectedLogLevel = LogLevel.Information;
            var expectedRule = await CreateRule();
            expectedRule.IsExecuting = false;
            
            var expectedPeriod = expectedRule.User.ActivePaymentPackageType
                .GetRestrictions()
                .MinNotificationRuleTimeout;
            var expectedExecutionTime = DateTime.UtcNow.AddSeconds(-expectedPeriod - 5);
            
            expectedRule.Period = expectedPeriod;
            expectedRule.LastExecutionTime = expectedExecutionTime;
            await CommandBuilder.SaveAsync(expectedRule);

            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            await CommitDbChanges();
            
            await _processingService.FindAndProcessWaitingRules();
            
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            DbSessionProvider.CurrentSession.Clear();
            var actualRule = await QueryBuilder.FindByIdAsync<NotificationRuleEntity>(expectedRule.Id);
            Assert.True(actualRule.LastExecutionTime > expectedExecutionTime);
            Assert.False(actualRule.IsExecuting);
        }
        
        [Fact]
        public async Task ShouldProcessWaitingRulesWithSeveralEmails()
        {
            await RulesAsExecutingAsync();

            var expectedCount = 2;
            var expectedLogLevel = LogLevel.Information;
            
            var expectedRule = await CreateRule();
            
            var expectedPeriod = expectedRule.User.ActivePaymentPackageType
                .GetRestrictions()
                .MinNotificationRuleTimeout; // 5 minutes
            var expectedExecutionTime = DateTime.UtcNow.AddSeconds(-expectedPeriod - 5);
            
            var emailFactory = DataFactory.UserEmailFactory();
            var userEmail1 = await UserEmailService.AddAsync(expectedRule.User, emailFactory.Generate().Email);
            var userEmail2 = await UserEmailService.AddAsync(expectedRule.User, emailFactory.Generate().Email);
            expectedRule.AddEmail(userEmail1);
            expectedRule.AddEmail(userEmail2);
            
            expectedRule.IsExecuting = false;
            expectedRule.Period = expectedPeriod;
            expectedRule.LastExecutionTime = expectedExecutionTime;
            await CommandBuilder.SaveAsync(expectedRule);

            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            await CommitDbChanges();
            
            await _processingService.FindAndProcessWaitingRules();
            
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);
        }

        private async Task<NotificationRuleEntity> CreateRule(
            LogLevel logLevel = LogLevel.Information,
            ICollection<NotificationConditionEntity> conditions = null,
            LogicOperatorType logicOperatorType = LogicOperatorType.Conjunction
        )
        {
            var expectedPeriod = 5 * 60;
            var user = await DataSeeder.CreateActivatedUser();
            
            var message = DataFactory.MessageTemplateFactory().Generate();
            message.User = user.Original;
            await CommandBuilder.SaveAsync(message);

            if (conditions == null)
            {
                conditions = new List<NotificationConditionEntity>();
                var conditionsFactory = DataFactory.NotificationConditionFactory();
                for (int i = 0; i < 3; i++)
                {
                    var condition = conditionsFactory.Generate();
                    condition.Value = logLevel.GetLabel();
                    conditions.Add(condition);
                }    
            }
            
            return await _notificationRuleService.SetRule(
                user.Original,
                _ruleFactory.Generate().Title,
                user.ActivePaymentPackageType.GetRestrictions().MinNotificationRuleTimeout,
                logicOperatorType,
                NotificationType.Email,
                message,
                conditions,
                user.Application
            );
        }

        private async Task RulesAsExecutingAsync()
        {
            await DbSessionProvider.CurrentSession.Query<NotificationRuleEntity>()
                .UpdateAsync(entity => new
                {
                    IsExecuting = true,
                });
            await CommitDbChanges();
        }
    }
}
