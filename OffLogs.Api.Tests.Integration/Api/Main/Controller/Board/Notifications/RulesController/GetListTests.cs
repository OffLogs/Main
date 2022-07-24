using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Notifications.RulesController
{
    public class GetListTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationRulesList;

        private const int DefaultPeriod = 300;

        private readonly Faker<MessageTemplateEntity> _messageFactory;
        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        private readonly INotificationRuleService _notificationRuleService;
        private readonly MessageTemplateEntity _expectedMessageTemplate;

        public GetListTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _messageFactory = DataFactory.MessageTemplateFactory();
            _ruleFactory = DataFactory.NotificationRuleFactory();
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
        }

        [Fact]
        public async Task OnlyAuthorizedUsersCanDoIt()
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new GetListRequest());
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldReceiveList()
        {
            var expectCount = 7;
            var user = await DataSeeder.CreateActivatedUser();

            for (int i = 1; i <= expectCount; i++)
            {
                await CreateRuleAsync(user);;
            }

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest());
            // Assert
            // response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<ListDto<NotificationRuleDto>>();
            Assert.Equal(expectCount, data.TotalCount);
            Assert.Equal(expectCount, data.Items.Count);
            Assert.NotEmpty(data.Items);
            Assert.All(data.Items, item =>
            {
                Assert.True(item.Id > 0);
                Assert.NotNull(item.MessageTemplate);
                Assert.NotNull(item.Application);
                Assert.True(item.Period > 0);
                Assert.True(item.Conditions.Count > 0);
            });
        }
        
        [Fact]
        public async Task ShouldReceiveOnlyForOwner()
        {
            var expectCount = 7;
            var user = await DataSeeder.CreateActivatedUser();

            for (int i = 1; i <= expectCount; i++)
            {
                await CreateRuleAsync(user);;
            }
            
            var user2 = await DataSeeder.CreateActivatedUser();
            for (int i = 1; i <= 3; i++)
            {
                await CreateRuleAsync(user2);;
            }

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetListRequest());
            // Assert
            var data = await response.GetJsonDataAsync<ListDto<NotificationRuleDto>>();
            Assert.Equal(expectCount, data.TotalCount);
            Assert.Equal(expectCount, data.Items.Count);
            Assert.NotEmpty(data.Items);
        }
        
        private async Task<NotificationRuleEntity> CreateRuleAsync(UserTestModel user)
        {
            var conditions = new List<NotificationConditionEntity>();
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            for (int i = 0; i < 3; i++)
            {
                var condition = conditionsFactory.Generate();
                condition.Value = LogLevel.Warning.GetLabel();
                conditions.Add(condition);
            } 
            
            var messageTemplate = _messageFactory.Generate();
            messageTemplate.User = user;
            CommandBuilder.SaveAsync(messageTemplate).Wait();
            
            return await _notificationRuleService.SetRule(
                user,
                _ruleFactory.Generate().Title,
                user.ActivePaymentPackageType.GetRestrictions().MinNotificationRuleTimeout,
                LogicOperatorType.Conjunction,
                NotificationType.Email,
                messageTemplate,
                conditions,
                user.Application
            );
        }
    }
}
