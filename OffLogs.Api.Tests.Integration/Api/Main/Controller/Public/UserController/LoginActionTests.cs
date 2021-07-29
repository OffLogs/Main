﻿using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.UserController
{
    public class LoginActionTests : MyApiIntegrationTest
    {
        public LoginActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData("/user/login")]
        public async Task ShouldNotLoginIfNotExists(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new
            {
                UserName = "test_not_exists",
                Password = "test_not_exists",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/user/login")]
        public async Task ShouldNotLoginIfPasswordIsIncorrect(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new
            {
                user.UserName,
                Password = "fake password",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/user/login")]
        public async Task ShouldLogin(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new
            {
                user.UserName,
                user.Password,
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var data = await response.GetJsonDataAsync<LoginResponseDto>();
            Assert.True(JwtAuthService.IsValidJwt(data.Token));
        }
    }
}