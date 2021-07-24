using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController
{
    public class CheckIsLoggedInActionTests : MyApiIntegrationTest
    {
        public CheckIsLoggedInActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData("/user/checkIsLoggedIn")]
        public async Task ShouldReturnUnauthorized(string url)
        {
            // Act
            var response = await GetRequestAsAnonymousAsync(url);
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/user/checkIsLoggedIn")]
        public async Task ShouldReturnUnauthorizedForIncorrectJwt(string url)
        {
            // Act
            var response = await GetRequestAsync(url, "badJwt");
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/user/checkIsLoggedIn")]
        public async Task ShouldReturnSuccess(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            // Act
            var response = await GetRequestAsync(url, user.ApiToken);
            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
