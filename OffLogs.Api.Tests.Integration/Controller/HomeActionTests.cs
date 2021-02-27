using System.Threading.Tasks;
using OffLogs.Business.Test.Extensions;
using Vizit.Api.Mobile.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller
{
    public class HomeActionTests: MyIntegrationTest
    {
        public HomeActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("/ping")]
        public async Task ShouldPingViberBot(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.GetJsonDataAsync<object>();
            Assert.Equal(
                new { Pong = true },
                responseData.Data
            );
        }
    }
}
