using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Entities.NotificationRules
{
    public class NotificationRuleServiceTests : MyApiIntegrationTest
    {
        private readonly INotificationRuleService _notificationRuleService;
        private readonly Faker<NotificationRuleEntity> _ruleFactory;

        public UserTestModel UserModel { get; set; }

        public NotificationRuleServiceTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
            
            _ruleFactory = DataFactory.NotificationRuleFactory();
            UserModel = DataSeeder.CreateActivatedUser().Result;
        }

        [Fact]
        public async Task ShouldCreateRule()
        {
            var expectedPeriod = PaymentPackageType.Basic.GetRestrictions().MinNotificationRuleTimeout;
            var expectedOperator = LogicOperatorType.Conjunction;

            var actualRule = await CreateRule();
            
            Assert.NotEmpty(actualRule.MessageTemplate.Subject);
            Assert.Equal(expectedPeriod, actualRule.Period);
            Assert.Equal(expectedOperator, actualRule.LogicOperator);
            Assert.True(actualRule.LastExecutionTime > DateTime.MinValue);
            Assert.True(actualRule.Id > 0);
            Assert.False(actualRule.IsExecuting);
        }

        [Fact]
        public async Task ShouldUpdateRule()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var actualRule = await CreateRule();
            var expectedRule = await CreateRule();
            
            var rule = await _notificationRuleService.SetRule(
                expectedRule.User,
                expectedRule.Title,
                expectedRule.Period,
                expectedRule.LogicOperator,
                expectedRule.Type,
                expectedRule.MessageTemplate,
                expectedRule.Conditions,
                expectedRule.Application,
                actualRule.Id
            );
            
            Assert.Equal(actualRule.Application.Id, expectedRule.Application.Id);
            Assert.Equal(actualRule.User.Id, expectedRule.User.Id);
            Assert.Equal(actualRule.MessageTemplate, expectedRule.MessageTemplate);
            Assert.Equal(actualRule.Period, expectedRule.Period);
            Assert.Equal(actualRule.Type, expectedRule.Type);
        }
        
        [Fact]
        public async Task ShouldReceiveNextRule()
        {
            await RulesAsExecutingAsync();
            
            var expectedPeriod = 5 * 60; // 5 minutes

            var expectedRule = await CreateRule();
            expectedRule.IsExecuting = false;
            expectedRule.Period = expectedPeriod;
            expectedRule.LastExecutionTime = DateTime.UtcNow.AddSeconds(-expectedPeriod - 5);
            await CommandBuilder.SaveAsync(expectedRule);
            
            await DbSessionProvider.PerformCommitAsync();
            var actualRule = await _notificationRuleService.GetNextAndSetExecutingAsync();
            Assert.NotNull(actualRule);
        }
        
        [Fact]
        public async Task ShouldNotReceiveNextRule()
        {
            await RulesAsExecutingAsync();

            var expectedRule = await CreateRule();
            
            var expectedPeriod = 5 * 60; // 5 minutes
            var timeNow = DateTime.UtcNow;
            var lastExecutionTime = timeNow.AddSeconds(-expectedPeriod + 10);
            
            expectedRule.IsExecuting = false;
            expectedRule.Period = expectedPeriod;
            expectedRule.LastExecutionTime = lastExecutionTime;
            await CommandBuilder.SaveAsync(expectedRule);

            await DbSessionProvider.PerformCommitAsync();
            var actualRule = await _notificationRuleService.GetNextAndSetExecutingAsync();
            Assert.Null(actualRule);
        }
        
        [Fact]
        public async Task ShouldReceiveRuleDataWithDisjunctionOperator()
        {
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            
            var expectedLogLevel = LogLevel.Warning;
            var expectedCount = 3;
            
            var conditions = new List<NotificationConditionEntity>();
            var condition = conditionsFactory.Generate();
            condition.Value = LogLevel.Error.GetLabel();
            conditions.Add(condition);
            
            condition = conditionsFactory.Generate();
            condition.Value = LogLevel.Information.GetLabel();
            conditions.Add(condition);
            
            condition = conditionsFactory.Generate();
            condition.Value = expectedLogLevel.GetLabel();
            conditions.Add(condition);
            
            var expectedRule = await CreateRule(
                expectedLogLevel,
                conditions,
                LogicOperatorType.Disjunction
            );
            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            
            var data = await _notificationRuleService.GetDataForNotificationRule(expectedRule);
            Assert.Equal(expectedCount, data.LogCount);
        }
        
        [Fact]
        public async Task ShouldReceiveRuleDataWithConjunctionOperator()
        {
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            
            var expectedLogLevel = LogLevel.Warning;
            var expectedCount = 3;
            
            var conditions = new List<NotificationConditionEntity>();
            var condition = conditionsFactory.Generate();
            condition.Value = expectedLogLevel.GetLabel();
            conditions.Add(condition);

            var expectedRule = await CreateRule(
                expectedLogLevel,
                conditions,
                LogicOperatorType.Conjunction
            );
            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            
            var data = await _notificationRuleService.GetDataForNotificationRule(expectedRule);
            Assert.Equal(expectedCount, data.LogCount);
        }
        
        [Fact]
        public async Task ShouldNotReceiveRuleDataWithConjunctionOperator()
        {
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            
            var expectedLogLevel = LogLevel.Warning;
            var expectedCount = 3;
            
            var conditions = new List<NotificationConditionEntity>();
            var condition = conditionsFactory.Generate();
            condition.Value = expectedLogLevel.GetLabel();
            conditions.Add(condition);
            
            condition = conditionsFactory.Generate();
            condition.Value = LogLevel.Error.GetLabel();
            conditions.Add(condition);

            var expectedRule = await CreateRule(
                expectedLogLevel,
                conditions,
                LogicOperatorType.Conjunction
            );
            await DataSeeder.CreateLogsAsync(
                expectedRule.Application.Id,
                expectedLogLevel,
                expectedCount
            );
            
            var data = await _notificationRuleService.GetDataForNotificationRule(expectedRule);
            Assert.Equal(0, data.LogCount);
        }
        
        private async Task<NotificationRuleEntity> CreateRule(
            LogLevel logLevel = LogLevel.Information,
            ICollection<NotificationConditionEntity> conditions = null,
            LogicOperatorType logicOperatorType = LogicOperatorType.Conjunction
        )
        {
            
            var message = DataFactory.MessageTemplateFactory().Generate();
            message.User = UserModel;
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
                UserModel,
                _ruleFactory.Generate().Title,
                UserModel.ActivePaymentPackageType.GetRestrictions().MinNotificationRuleTimeout,
                logicOperatorType,
                NotificationType.Email,
                message,
                conditions,
                UserModel.Application
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
