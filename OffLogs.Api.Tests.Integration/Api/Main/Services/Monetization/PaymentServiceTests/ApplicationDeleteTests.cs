using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Monetization.PaymentServiceTests
{
    public class GetActivePackageTypeTests : MyApiIntegrationTest
    {
        private readonly UserEntity _user;

        public GetActivePackageTypeTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _user = (DataSeeder.CreateActivatedUser().Result).Original;
        }

        [Fact]
        public void ShouldReceiveBasicPlanIfPackageNotFound()
        {
            Assert.Empty(_user.PaymentPackages);
            var activePackage = PaymentService.GetActivePackageType(_user);
            Assert.Equal(PaymentPackageType.Basic, activePackage);
        }
        
        [Fact]
        public async Task ShouldReceiveProPlanIfExistsAndNotExpired()
        {
            var paymentPackage = new UserPaymentPackageEntity()
            {
                Type = PaymentPackageType.Pro,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            };
            paymentPackage.SetUser(_user);
            await CommitDbChanges();

            var actualUser = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            Assert.NotEmpty(actualUser.PaymentPackages);
            var activePackage = PaymentService.GetActivePackageType(actualUser);
            Assert.Equal(PaymentPackageType.Pro, activePackage);
        }
        
        [Fact]
        public async Task ShouldReceiveBasicPlanIfCurrentExpired()
        {
            var paymentPackage = new UserPaymentPackageEntity()
            {
                Type = PaymentPackageType.Pro,
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddDays(-1)
            };
            paymentPackage.SetUser(_user);
            await CommitDbChanges();

            var actualUser = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            Assert.NotEmpty(actualUser.PaymentPackages);
            var activePackage = PaymentService.GetActivePackageType(actualUser);
            Assert.Equal(PaymentPackageType.Basic, activePackage);
        }
    }
}
