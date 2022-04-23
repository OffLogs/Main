using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
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
            
            var actualRule = await NotificationRuleService.CreateRule(
                user,
                expectedPeriod,
                LogicOperatorType.And,
                message,
                expectedConditions,
                user.Application
            );
            
            Assert.Equal(message.Subject, actualRule.Message.Subject);
            Assert.Equal(expectedPeriod, actualRule.Period);
            Assert.Equal(expectedOperator, actualRule.LogicOperator);
            Assert.True(actualRule.LastExecutionTime > DateTime.MinValue);
            Assert.True(actualRule.Id > 0);
        }
    }
}
