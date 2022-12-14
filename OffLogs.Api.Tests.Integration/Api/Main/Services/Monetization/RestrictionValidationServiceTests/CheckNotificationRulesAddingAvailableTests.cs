using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities.Notifications;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Monetization;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Monetization.RestrictionValidationServiceTests
{
    public class CheckNotificationRulesAddingAvailableTests : MyApiIntegrationTest
    {
        private readonly UserEntity _user;
        private readonly UserTestModel _userModel;
        private readonly IRestrictionValidationService _restrictionValidationService;
        private readonly INotificationRuleService _notificationRuleService;
        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        private readonly Faker<MessageTemplateEntity> _templateFactory;
        private readonly IPaymentPackageService _paymentPackageService;

        public CheckNotificationRulesAddingAvailableTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _userModel = DataSeeder.CreateActivatedUser().Result;
            _user = _userModel.Original;
            
            _restrictionValidationService = _factory.Services.GetRequiredService<IRestrictionValidationService>();
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
            _paymentPackageService = _factory.Services.GetRequiredService<IPaymentPackageService>();
            _ruleFactory = DataFactory.NotificationRuleFactory();
            _templateFactory = DataFactory.MessageTemplateFactory();
        }

        [Theory]
        [InlineData(PaymentPackageType.Pro, 30)]
        public async Task ShouldNotThrowExceptionIfMaxRulesCounterIsZero(PaymentPackageType packageType, int maxRules)
        {
            await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);
            for (int i = 0; i < maxRules; i++)
            {
                await CreateRule();
            }
            await CommitDbChanges();
            
            await DbSessionProvider.CurrentSession.RefreshAsync(_user);
            
            var newRule = new NotificationRuleEntity();
            newRule.Period = packageType.GetRestrictions().MinNotificationRuleTimeout;
            newRule.User = _user;
            _restrictionValidationService.CheckNotificationRulesAddingAvailable(newRule);;
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic, 5)]
        [InlineData(PaymentPackageType.Standart, 20)]
        public async Task ShouldThrowExceptionIfMaxRulesCounterMoreThanPackageRestriction(PaymentPackageType packageType, int createdRules)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            for (int i = 0; i < createdRules; i++)
            {
                await CreateRule();
            }
            await CommitDbChanges();
            
            await Assert.ThrowsAsync<PaymentPackageRestrictionException>(async () =>
            {
                var user = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
                
                var newRule = new NotificationRuleEntity();
                newRule.Period = user.ActivePaymentPackageType.GetRestrictions().MinNotificationRuleTimeout;
                newRule.User = user;
                _restrictionValidationService.CheckNotificationRulesAddingAvailable(newRule);
            });
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic, 60 * 60 * 6 - 1)]
        [InlineData(PaymentPackageType.Standart, 60 * 30 - 1)]
        [InlineData(PaymentPackageType.Pro, 60 * 5 - 1)]
        public async Task ShouldThrowExceptionIfMaxTimeoutLessThanPackageRestriction(PaymentPackageType packageType, int timeout)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            await CommitDbChanges();
            
            Assert.Throws<PaymentPackageRestrictionException>( () =>
            {
                var newRule = new NotificationRuleEntity();
                newRule.Period = timeout;
                newRule.User = _user;
                _restrictionValidationService.CheckNotificationRulesAddingAvailable(newRule);
            });
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic, 60 * 60 * 6)]
        [InlineData(PaymentPackageType.Standart, 60 * 30)]
        [InlineData(PaymentPackageType.Pro, 60 * 5)]
        public async Task ShouldNotThrowExceptionIfCorrectMaxTimeout(PaymentPackageType packageType, int timeout)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            await CommitDbChanges();
            
            var newRule = new NotificationRuleEntity();
            newRule.Period = timeout;
            newRule.User = _user;
            _restrictionValidationService.CheckNotificationRulesAddingAvailable(newRule);
        }
        
        private async Task<NotificationRuleEntity> CreateRule(int? period = null)
        {
            period ??= _user.ActivePaymentPackageType.GetRestrictions().MinNotificationRuleTimeout;
            
            var message = DataFactory.MessageTemplateFactory().Generate();
            message.User = _user;
            await CommandBuilder.SaveAsync(message);

            var conditions = new List<NotificationConditionEntity>();
            var conditionsFactory = DataFactory.NotificationConditionFactory();
            var condition = conditionsFactory.Generate();
            condition.Value = LogLevel.Information.GetLabel();
            conditions.Add(condition);
            
            return await _notificationRuleService.SetRule(
                _user,
                _ruleFactory.Generate().Title,
                period.Value,
                LogicOperatorType.Conjunction,
                NotificationType.Email,
                message,
                conditions,
                _userModel.Application
            );
        }
    }
}
