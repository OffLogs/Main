﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Models.Request.User;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller.UserController
{
    public class LoginActionTests: MyIntegrationTest
    {
        public LoginActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
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
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
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
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
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
            var data = await response.GetJsonDataAsync<LoginResponseModel>();
            Assert.True(JwtAuthService.IsValidJwt(data.Data.Token));
        }
    }
}
