namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public
{
    public class HomeActionTests : MyApiIntegrationTest
    {
        public HomeActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        // TODO: Revert it
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
