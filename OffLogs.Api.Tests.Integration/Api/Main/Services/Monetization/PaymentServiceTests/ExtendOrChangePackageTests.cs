using System;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Monetization.PaymentServiceTests
{
    public class ExtendOrChangePackageTests : MyApiIntegrationTest
    {
        private readonly UserEntity _user;

        public ExtendOrChangePackageTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _user = (DataSeeder.CreateActivatedUser().Result).Original;
        }

        [Fact]
        public async Task ShouldThrowExceptionIfIncorrectPackageType()
        {
            Assert.Empty(_user.PaymentPackages);
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await PaymentPackageService.ExtendOrChangePackage(_user, PaymentPackageType.Basic, 1);
            });
        }
        
        [Theory]
        [InlineData(PaymentPackageType.Standart, 10, 31)]
        [InlineData(PaymentPackageType.Standart, 20, 62)]
        [InlineData(PaymentPackageType.Standart, 15, 31 + 16)]
        [InlineData(PaymentPackageType.Standart, 15.5, 31 + 18)]
        [InlineData(PaymentPackageType.Pro, 30, 31)]
        [InlineData(PaymentPackageType.Pro, 31, 33)]
        [InlineData(PaymentPackageType.Pro, 45, 31 + 16)]
        [InlineData(PaymentPackageType.Pro, 60, 31 * 2)]
        public async Task ShouldReceiveProPlanIfExistsAndNotExpired(PaymentPackageType type, decimal amount, int paidDays)
        {
            await PaymentPackageService.ExtendOrChangePackage(_user, type, amount);
            await CommitDbChanges();

            var actualUser = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            var actualPackage = actualUser.LastPaymentPackage;

            Assert.Equal(type, actualPackage.Type);
            Assert.Equal(paidDays, (actualPackage.ExpirationDate - DateTime.UtcNow.Date).Days);
        }
        
        [Theory]
        [InlineData(31, 30, 42)]
        [InlineData(62, 31, 53)]
        [InlineData(5, 30, 31 + 2)]
        [InlineData(31, 30, 31 + 11)]
        public async Task ShouldRecalculateOldPlanAndIncrementDaysCountForNewPlan(int previousPlanLeftDays, decimal newPlanAmount, int expectedPaidDays)
        {
            var expectedPreviousPlanType = PaymentPackageType.Standart;
            var expectedNextPlanType = PaymentPackageType.Pro;
            
            await PaymentPackageService.ExtendOrChangePackage(
                _user,
                expectedPreviousPlanType,
                10
            );
            _user.LastPaymentPackage.ExpirationDate = DateTime.UtcNow.Date.AddDays(previousPlanLeftDays);
            await DbSessionProvider.CurrentSession.UpdateAsync(_user);

            await PaymentPackageService.ExtendOrChangePackage(_user, expectedNextPlanType, newPlanAmount);
            await CommitDbChanges();

            var actualUser = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            var actualPackage = actualUser.LastPaymentPackage;
            var actualPreviousPackage = actualUser.PreviousPaymentPackage;

            Assert.Equal(expectedNextPlanType, actualPackage.Type);
            Assert.Equal(expectedPreviousPlanType, actualPreviousPackage.Type);
            
            Assert.Equal(expectedPaidDays, actualPackage.LeftPaidDays);
        }
        
        [Fact]
        public async Task ShouldMarkPreviousPlanAsExpired()
        {
            var expectedPreviousPlanType = PaymentPackageType.Pro;
            var expectedNextPlanType = PaymentPackageType.Standart;
            
            await PaymentPackageService.ExtendOrChangePackage(
                _user,
                expectedPreviousPlanType,
                10
            );

            await PaymentPackageService.ExtendOrChangePackage(_user, expectedNextPlanType, 10);
            await CommitDbChanges();

            var actualUser = await QueryBuilder.FindByIdAsync<UserEntity>(_user.Id);
            var actualPackage = actualUser.LastPaymentPackage;
            var actualPreviousPackage = actualUser.PreviousPaymentPackage;

            Assert.Equal(expectedNextPlanType, actualPackage.Type);
            Assert.True(actualPackage.ExpirationDate > DateTime.UtcNow.Date);
            
            Assert.Equal(expectedPreviousPlanType, actualPreviousPackage.Type);
            Assert.True(actualPreviousPackage.ExpirationDate <= DateTime.UtcNow.Date);
        }
    }
}
