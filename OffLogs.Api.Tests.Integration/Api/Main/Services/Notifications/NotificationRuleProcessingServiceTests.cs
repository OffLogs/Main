using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Notifications;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Notifications
{
    public class NotificationRuleProcessingServiceTests : MyApiIntegrationTest
    {
        protected readonly INotificationRuleService NotificationRuleService;
        protected readonly INotificationRuleProcessingService ProcessingService;
        
        public NotificationRuleProcessingServiceTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            NotificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
            ProcessingService = _factory.Services.GetRequiredService<INotificationRuleProcessingService>();
        }

        [Fact]
        public async Task ShouldProcessWaitingRules()
        {
            await RulesAsExecutingAsync();

            var expectedCount = 2;
            var expectedLogLevel = LogLevel.Information;
            var expectedPeriod = 5 * 60; // 5 minutes

            var expectedRule = await CreateRule();
            expectedRule.IsExecuting = false;
            expectedRule.Period = expectedPeriod;
            expectedRule.LastExecutionTime = DateTime.UtcNow.AddSeconds(-expectedPeriod - 5);
            await CommandBuilder.SaveAsync(expectedRule);

            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            
            await DbSessionProvider.PerformCommitAsync();
            await ProcessingService.FindAndProcessWaitingRules();

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
            
            var message = DataFactory.NotificationMessageFactory().Generate();
            message.User = user;
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
            
            return await NotificationRuleService.SetRule(
                user,
                expectedPeriod,
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
            await DbSessionProvider.PerformCommitAsync();
        }
    }
}
