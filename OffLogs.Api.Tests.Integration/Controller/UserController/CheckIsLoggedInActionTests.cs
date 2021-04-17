using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller.UserController
{
    public class CheckIsLoggedInActionTests: MyIntegrationTest
    {
        public CheckIsLoggedInActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
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
