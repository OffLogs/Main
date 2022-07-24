using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
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
        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        private readonly INotificationRuleService _notificationRuleService;
        private readonly MessageTemplateEntity _expectedMessageTemplate;
        private UserTestModel _userModel { get; set; }

        public SetRuleTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _userModel = DataSeeder.CreateActivatedUser().Result;
            _messageFactory = DataFactory.MessageTemplateFactory();
            _ruleFactory = DataFactory.NotificationRuleFactory();
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
            var expectedTitle = _ruleFactory.Generate().Title;

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
                Conditions = conditions,
                Title = expectedTitle
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.True(data.Id > 0);
            Assert.Equal(DefaultPeriod, data.Period);
            Assert.Equal(_expectedMessageTemplate.Id, data.MessageTemplate.Id);
            Assert.Equal(conditions.Count, data.Conditions.Count);
            Assert.Equal(expectedOperator, data.LogicOperator);
            Assert.Equal(expectedTitle, data.Title);
        }
        
        [Fact]
        public async Task ShouldAddNewIfIdIsZero()
        {
            var expectedOperator = LogicOperatorType.Conjunction;
            var expectedTitle = _ruleFactory.Generate().Title;

            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Warning"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Id = 0,
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = expectedTitle
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.True(data.Id > 0);
            Assert.Equal(DefaultPeriod, data.Period);
            Assert.Equal(_expectedMessageTemplate.Id, data.MessageTemplate.Id);
            Assert.Equal(conditions.Count, data.Conditions.Count);
            Assert.Equal(expectedOperator, data.LogicOperator);
            Assert.Equal(expectedTitle, data.Title);
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
                Conditions = conditions,
                Title = _ruleFactory.Generate().Title
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
                Conditions = conditions,
                Title = _ruleFactory.Generate().Title
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task ShouldFailIfConditionsTooMuch()
        {
            var expectedOperator = LogicOperatorType.Conjunction;
            var expectedTitle = _ruleFactory.Generate().Title;

            var conditions = new List<SetConditionRequest>();
            for (var i = 0; i < 11; i++)
            {
                conditions.Add(
                    new SetConditionRequest { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"}
                );
            }

            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = expectedTitle
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var data = await response.GetDataAsStringAsync();
            Assert.Contains("The field Conditions must be a string or array type with a maximum length of", data);
        }
        
        [Fact]
        public async Task ShouldAddNewWithAdditionalEmails()
        {
            var emailFactory = DataFactory.UserEmailFactory();
            var userEmail1 = await UserEmailService.AddAsync(_userModel.Original, emailFactory.Generate().Email);
            var userEmail2 = await UserEmailService.AddAsync(_userModel.Original, emailFactory.Generate().Email);
            await CommitDbChanges();
            
            var expectedOperator = LogicOperatorType.Conjunction;
            var expectedTitle = _ruleFactory.Generate().Title;

            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = expectedTitle,
                UserEmailIds = new []{ userEmail1.Id, userEmail2.Id }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.Equal(2, data.Emails.Count);
        }
        
        [Fact]
        public async Task ShouldAddNewWithAdditionalEmailsAndDoNotAddOtherEmails()
        {
            var userModel2 = await DataSeeder.CreateActivatedUser();
            
            var emailFactory = DataFactory.UserEmailFactory();
            var userEmail1 = await UserEmailService.AddAsync(_userModel.Original, emailFactory.Generate().Email);
            var userEmail2 = await UserEmailService.AddAsync(userModel2.Original, emailFactory.Generate().Email);
            await CommitDbChanges();
            
            var expectedOperator = LogicOperatorType.Conjunction;
            var expectedTitle = _ruleFactory.Generate().Title;

            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = expectedTitle,
                UserEmailIds = new []{ userEmail1.Id, userEmail2.Id }
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.Equal(1, data.Emails.Count);
            Assert.Equal(userEmail1.Id, data.Emails.First().Id);
        }
        
        [Fact]
        public async Task ShouldRemoveEmailsFromRule()
        {
            var actualRule = await CreateRuleAsync();
            var emailFactory = DataFactory.UserEmailFactory();
            var userEmail1 = await UserEmailService.AddAsync(_userModel.Original, emailFactory.Generate().Email);
            var userEmail2 = await UserEmailService.AddAsync(_userModel.Original, emailFactory.Generate().Email);
            
            actualRule.AddEmail(userEmail1);
            actualRule.AddEmail(userEmail2);
            
            await CommandBuilder.SaveAsync(actualRule);
            
            await CommitDbChanges();
            
            var expectedOperator = LogicOperatorType.Conjunction;
            var expectedTitle = _ruleFactory.Generate().Title;

            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Id = actualRule.Id,
                Period = DefaultPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = expectedTitle,
                UserEmailIds = Array.Empty<long>()
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<NotificationRuleDto>();
            Assert.Equal(0, data.Emails.Count);

            // Emails should not be deleted from user
            await CommitDbChanges();

            var user1 = await QueryBuilder.FindByIdAsync<UserEntity>(_userModel.Id);
            Assert.Equal(2, user1.Emails.Count);
            
            var rule = await QueryBuilder.FindByIdAsync<NotificationRuleEntity>(actualRule.Id);
            Assert.Equal(0, rule.Emails.Count);
        }
        
        [Fact]
        public async Task ShouldReturnErrorIfPackageRestrictions()
        {
            var expectedPeriod = 2000;
            var expectedOperator = LogicOperatorType.Conjunction;

            for (int i = 0; i <= 4; i++)
            {
                await CreateRuleAsync();
            }
            
            var conditions = new List<SetConditionRequest>()
            {
                new() { ConditionField = ConditionFieldType.LogLevel.ToString(), Value = "Information"},
            };
            
            // Act
            var response = await PostRequestAsync(Url, _userModel.ApiToken, new SetRuleRequest
            {
                Period = expectedPeriod,
                ApplicationId = _userModel.ApplicationId,
                Type = NotificationType.Email.ToString(),
                LogicOperator = expectedOperator.ToString(),
                TemplateId = _expectedMessageTemplate.Id,
                Conditions = conditions,
                Title = _ruleFactory.Generate().Title
            });
            // Assert
            var responseData = await response.GetJsonErrorAsync();
            Assert.Equal(new PaymentPackageRestrictionException().GetTypeName(), responseData.Type);
        }
        
        private async Task<NotificationRuleEntity> CreateRuleAsync(
            LogLevel logLevel = LogLevel.Information
        )
        {
            var expectedPeriod = _userModel.Original.ActivePaymentPackageType
                .GetRestrictions()
                .FavoriteLogsExpirationTimeout;
            var fakeRule = _ruleFactory.Generate();

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
                fakeRule.Title,
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
