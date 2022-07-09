using System;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class GetOneActionTests: MyApiIntegrationTest
    {
        public GetOneActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        #region Common
        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task OnlyAuthorizedUsersCanGetApplications(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetRequest()
            {
                Id = user.ApplicationId
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task ShouldNotGetForOtherUser(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetRequest()
            {
                Id = user2.ApplicationId
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task ShouldReceiveApplication(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.Equal(user1.Application.Id, responseData.Id);
            Assert.Equal(user1.Application.Name, responseData.Name);
            Assert.Equal(user1.Application.ApiToken, responseData.ApiToken);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task SharedUserShouldReceiveApplication(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user1.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new GetRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.Equal(user1.Application.Id, responseData.Id);
            Assert.Equal(user1.Application.Name, responseData.Name);
            Assert.Equal(user1.Application.ApiToken, responseData.ApiToken);
        }
        #endregion

        #region Permissions
        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task OwnerShouldReceiveIsWriteAccessAsTrue(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.Equal(user1.Application.Id, responseData.Id);
            Assert.True(responseData.Permissions.IsHasReadAccess);
            Assert.True(responseData.Permissions.IsHasWriteAccess);
        }

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task SharedUserShouldReceiveIsReadAccessAsTrue(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user1.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new GetRequest()
            {
                Id = user1.ApplicationId
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.Equal(user1.Application.Id, responseData.Id);
            Assert.True(responseData.Permissions.IsHasReadAccess);
            Assert.False(responseData.Permissions.IsHasWriteAccess);
        }
        #endregion
    }
}
