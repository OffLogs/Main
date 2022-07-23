using System.Threading.Tasks;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Redis
{
    public class CommonCommunicationTests: MyApiIntegrationTest
    {
        public CommonCommunicationTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendAndReceiveStringValue()
        {
            var expectedKey = nameof(CommonCommunicationTests) + "-test-key";
            var expectedValue = "some value";

            await RedisClient.SetString(expectedKey, expectedValue);

            var actualValue = await RedisClient.GetString(expectedKey);
            
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
