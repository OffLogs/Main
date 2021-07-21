using System.Net.Http.Headers;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Public.Home.Actions;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller
{
    public class HomeActionTests: MyApiIntegrationTest
    {
        public HomeActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        // [Theory]
        // [InlineData("/ping")]
        // public async Task ShouldPingViberBot(string url)
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();
        //
        //     // Act
        //     var response = await client.GetAsync(url);
        //
        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     var responseData = await response.GetJsonDataAsync<PongResponse>();
        //     Assert.Equal(
        //         new PongResponse(),
        //         responseData.Data
        //     );
        // }
        //
        // [Theory]
        // [InlineData("/application-auth-ping")]
        // public async Task ShouldPingAsAuthorizedApplication(string url)
        // {
        //     var jwtService = _factory.Services.GetService(typeof(IJwtApplicationService)) as IJwtApplicationService;
        //     var token = jwtService.BuildJwt(123);
        //     // Arrange
        //     var client = _factory.CreateClient();
        //     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //
        //     // Act
        //     var response = await client.GetAsync(url);
        //
        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     var responseData = await response.GetJsonDataAsync<PongResponse>();
        //     Assert.Equal(
        //         new PongResponse(),
        //         responseData.Data
        //     );
        // }
        //
        // [Theory]
        // [InlineData("/application-auth-ping")]
        // public async Task ShouldPingAsAuthorizedApplicationWithApiKey(string url)
        // {
        //     var jwtService = _factory.Services.GetService(typeof(IJwtApplicationService)) as IJwtApplicationService;
        //     var token = jwtService.BuildJwt(123);
        //     // Arrange
        //     var client = _factory.CreateClient();
        //
        //     // Act
        //     var response = await client.GetAsync(url + "?api_token=" + token);
        //
        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     var responseData = await response.GetJsonDataAsync<PongResponse>();
        //     Assert.Equal(
        //         new PongResponse(),
        //         responseData.Data
        //     );
        // }
    }
}
