using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Notifications.RulesController
{
    public partial class SetRuleTests : MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.NotificationRulesSet;

        private const int DefaultPeriod = 300;

        private readonly Faker<MessageTemplateEntity> _messageFactory;
        private readonly INotificationRuleService _notificationRuleService;
        private readonly MessageTemplateEntity _expectedMessageTemplate;
        private UserTestModel _userModel { get; set; }

        public SetRuleTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _userModel = DataSeeder.CreateActivatedUser().Result;
            _messageFactory = DataFactory.NotificationMessageFactory();
            _expectedMessageTemplate = _messageFactory.Generate();
            _expectedMessageTemplate.User = _userModel;
            CommandBuilder.SaveAsync(_expectedMessageTemplate).Wait();
            
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
        }

        [Fact]
        public async Task OnlyAuthorizedUsersCanDoIt()
        {
            var expectedMessage = _messageFactory.Generate();

            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new SetRuleRequest()
            {
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldContainAtLeastOneCondition()
        {
            var expectedOperator = LogicOperatorType.Conjunction;

            var conditions = new List<SetConditionRequest>(){};
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Conditions", await response.GetDataAsStringAsync());
        }
        
        [Fact]
        public async Task ShouldAddNew()
        {
            var expectedOperator = LogicOperatorType.Conjunction;

            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Warning"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.True(data.Id > 0);
            Assert.Equal(DefaultPeriod, data.Period);
            Assert.Equal(_expectedMessageTemplate.Id, data.MessageTemplate.Id);
            Assert.Equal(conditions.Count, data.Conditions.Count);
            Assert.Equal(expectedOperator, data.LogicOperator);
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            var expectedPeriod = 2000;
            var expectedOperator = LogicOperatorType.Conjunction;

            var actualRule = await CreateRuleAsync();
            
            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Warning"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Id = actualRule.Id,
                Period = expectedPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.Equal(actualRule.Id, data.Id);
            Assert.Equal(expectedPeriod, data.Period);
            Assert.Equal(_expectedMessageTemplate.Id, data.MessageTemplate.Id);
            Assert.Equal(conditions.Count, data.Conditions.Count);
            Assert.Equal(expectedOperator, data.LogicOperator);
        }
        
        [Fact]
        public async Task OnlyOwnerCanUpdate()
        {
            var expectedPeriod = 2000;
            var expectedOperator = LogicOperatorType.Conjunction;
            var user2 = await DataSeeder.CreateActivatedUser();

            var actualRule = await CreateRuleAsync();
            
            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Warning"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, user2.ApiToken, new SetRuleRequest
            {
                Id = actualRule.Id,
                Period = expectedPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        private async Task<NotificationRuleEntity> CreateRuleAsync(
            LogLevel logLevel = LogLevel.Information
        )
        {
            var expectedPeriod = 5 * 60;

            var conditions = new List<NotificationConditionEntity>();
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            for (int i = 0; i < 3; i++)
            {
                var condition = conditionsFactory.Generate();
                condition.Value = logLevel.GetLabel();
                conditions.Add(condition);
            } 
            
            return await _notificationRuleService.SetRule(
                _userModel,
                expectedPeriod,
                LogicOperatorType.Conjunction,
                NotificationType.Email,
                _expectedMessageTemplate,
                conditions,
                _userModel.Application
            );
        }
    }
}
