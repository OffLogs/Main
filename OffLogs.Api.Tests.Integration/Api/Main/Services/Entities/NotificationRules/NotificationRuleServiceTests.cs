using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Entities.NotificationRules
{
    public class NotificationRuleServiceTests : MyApiIntegrationTest
    {
        protected readonly INotificationRuleService NotificationRuleService;
        
        public NotificationRuleServiceTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            NotificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
        }

        [Fact]
        public async Task ShouldCreateRule()
        {
            var expectedPeriod = 5 * 60;
            var expectedOperator = LogicOperatorType.And;

            var actualRule = await CreateRule();
            
            Assert.NotEmpty(actualRule.Message.Subject);
            Assert.Equal(expectedPeriod, actualRule.Period);
            Assert.Equal(expectedOperator, actualRule.LogicOperator);
            Assert.True(actualRule.LastExecutionTime > DateTime.MinValue);
            Assert.True(actualRule.Id > 0);
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
            var actualRule = await NotificationRuleService.GetNextAndSetExecuting();
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
            var actualRule = await NotificationRuleService.GetNextAndSetExecuting();
            Assert.Null(actualRule);
        }
        
        private async Task<NotificationRuleEntity> CreateRule()
        {
            var expectedPeriod = 5 * 60;
            var user = await DataSeeder.CreateActivatedUser();
            
            var message = DataFactory.NotificationMessageFactory().Generate();
            message.User = user;
            await CommandBuilder.SaveAsync(message);

            var conditionsFactory = DataFactory.NotificationConditionFactory();
            var expectedConditions = new List<NotificationConditionEntity>();
            for (int i = 0; i < 3; i++)
            {
                expectedConditions.Add(conditionsFactory.Generate());
            }
            
            return await NotificationRuleService.CreateRule(
                user,
                expectedPeriod,
                LogicOperatorType.And,
                message,
                expectedConditions,
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
