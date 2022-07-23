using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Orm.Entities.User;
using OffLogs.Business.Services.Redis.Clients;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Redis.Clients
{
    public class UserInfoRedisClientTests: MyApiIntegrationTest
    {
        private readonly UserEntity _user;
        private readonly IUserInfoRedisClient _userInfoRedisClient;

        public UserInfoRedisClientTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _user = (DataSeeder.CreateActivatedUser().Result).Original;
            _userInfoRedisClient = _factory.Services.GetRequiredService<IUserInfoRedisClient>();
            
            PaymentPackageService.ExtendOrChangePackage(_user, PaymentPackageType.Pro, 30);
            CommitDbChanges().Wait();
        }

        [Fact]
        public async Task ShouldSeedAndReadPackageInfoFromRedis()
        {
            await _userInfoRedisClient.SeedUsersPackages();
            var actualPackage = await _userInfoRedisClient.GetUsersPaymentPackageType(_user.Id);
            
            Assert.Equal(PaymentPackageType.Pro, actualPackage);
        }
    }
}
