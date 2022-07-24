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
    public class CheckEmailsAddingAvailableTests : MyApiIntegrationTest
    {
        private readonly UserEntity _user;
        private readonly UserTestModel _userModel;
        private readonly IRestrictionValidationService _restrictionValidationService;
        private readonly INotificationRuleService _notificationRuleService;
        private readonly Faker<NotificationRuleEntity> _ruleFactory;
        private readonly Faker<MessageTemplateEntity> _templateFactory;
        private readonly IPaymentPackageService _paymentPackageService;
        private readonly Faker<UserEmailEntity> _emailFactory;

        public CheckEmailsAddingAvailableTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _userModel = DataSeeder.CreateActivatedUser().Result;
            _user = _userModel.Original;
            
            _restrictionValidationService = _factory.Services.GetRequiredService<IRestrictionValidationService>();
            _notificationRuleService = _factory.Services.GetRequiredService<INotificationRuleService>();
            _paymentPackageService = _factory.Services.GetRequiredService<IPaymentPackageService>();
            _ruleFactory = DataFactory.NotificationRuleFactory();
            _templateFactory = DataFactory.MessageTemplateFactory();
            _emailFactory = DataFactory.UserEmailFactory();
        }

        [Theory]
        [InlineData(PaymentPackageType.Pro, 30)]
        public async Task ShouldNotThrowExceptionIfMaxRulesCounterIsZero(PaymentPackageType packageType, int maxRules)
        {
            await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);
            for (int i = 0; i < maxRules; i++)
            {
                await CreateEmail();
            }
            await CommitDbChanges();
            
            var user = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            _restrictionValidationService.CheckUserEmailsAddingAvailable(user);;
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic, 5)]
        [InlineData(PaymentPackageType.Standart, 20)]
        public async Task ShouldThrowExceptionIfMaxRulesCounterMoreThanPackageRestriction(PaymentPackageType packageType, int maxRules)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            for (int i = 0; i < maxRules; i++)
            {
                await CreateEmail();
            }
            await CommitDbChanges();
            
            await Assert.ThrowsAsync<PaymentPackageRestrictionException>(async () =>
            {
                var user = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
                _restrictionValidationService.CheckUserEmailsAddingAvailable(user);
            });
        }
        
        private async Task CreateEmail()
        {
            var email = _emailFactory.Generate();
            email.SetUser(_user);
            await CommandBuilder.SaveAsync(_user);
        }
    }
}
