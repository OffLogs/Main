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
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.NotificationRule;
using OffLogs.Business.Services.Monetization;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Monetization.RestrictionValidationServiceTests
{
    public class CheckApplicationsAddingAvailableTests : MyApiIntegrationTest
    {
        private readonly UserEntity _user;
        private readonly UserTestModel _userModel;
        private readonly IRestrictionValidationService _restrictionValidationService;
        private readonly IPaymentPackageService _paymentPackageService;
        private readonly Faker<ApplicationEntity> _applicationFactory;

        public CheckApplicationsAddingAvailableTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _userModel = DataSeeder.CreateActivatedUser().Result;
            _user = _userModel.Original;
            
            _restrictionValidationService = _factory.Services.GetRequiredService<IRestrictionValidationService>();
            _factory.Services.GetRequiredService<INotificationRuleService>();
            _paymentPackageService = _factory.Services.GetRequiredService<IPaymentPackageService>();
            _applicationFactory = DataFactory.ApplicationFactory();
            _factory.Services.GetRequiredService<IApplicationService>();
        }

        [Theory]
        [InlineData(PaymentPackageType.Basic, 4)]
        [InlineData(PaymentPackageType.Standart, 14)]
        [InlineData(PaymentPackageType.Pro, 44)]
        public async Task ShouldThrowExceptionIfMaxRulesCounterMoreThanPackageRestriction(PaymentPackageType packageType, int addedItemCounter)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            for (int i = 0; i < addedItemCounter; i++)
            {
                await CreateApplication();
            }
            await CommitDbChanges();
            
            await Assert.ThrowsAsync<PaymentPackageRestrictionException>(async () =>
            {
                var user = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
                _restrictionValidationService.CheckApplicationsAddingAvailable(user);
            });
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Basic, 3)]
        [InlineData(PaymentPackageType.Standart, 13)]
        [InlineData(PaymentPackageType.Pro, 43)]
        public async Task ShouldNotThrowExceptionIfMaxRulesCounterMoreLessPackageRestriction(PaymentPackageType packageType, int addedItemCounter)
        {
            if (packageType != PaymentPackageType.Basic)
            {
                await _paymentPackageService.ExtendOrChangePackage(_user, packageType, 10);    
            }
            for (int i = 0; i < addedItemCounter; i++)
            {
                await CreateApplication();
            }
            await CommitDbChanges();
            
            var user = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            _restrictionValidationService.CheckApplicationsAddingAvailable(user);;
        }
        
        private async Task CreateApplication()
        {
            var application = _applicationFactory.Generate();
            _user.AddApplication(application);
            await CommandBuilder.SaveAsync(application);
        }
    }
}
