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
        private readonly IUserInfoProducerRedisClient _producer;
        private readonly IUserInfoConsumerRedisClient _consumer;

        public UserInfoRedisClientTests(ApiCustomWebApplicationFactory factory) : base(factory)
        {
            _user = (DataSeeder.CreateActivatedUser().Result).Original;
            _producer = _factory.Services.GetRequiredService<IUserInfoProducerRedisClient>();
            _consumer = _factory.Services.GetRequiredService<IUserInfoConsumerRedisClient>();
            
            PaymentPackageService.ExtendOrChangePackage(_user, PaymentPackageType.Pro, 30).Wait();
            CommitDbChanges().Wait();
        }

        [Fact]
        public async Task ShouldSeedAndReadPackageInfoFromRedis()
        {
            await _producer.SeedUsersPackages();
            var actualPackage = await _consumer.GetUsersPaymentPackageType(_user.Id);
            
            Assert.Equal(PaymentPackageType.Pro, actualPackage);
        }
    }
}
