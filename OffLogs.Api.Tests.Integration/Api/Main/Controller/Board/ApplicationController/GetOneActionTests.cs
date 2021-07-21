using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Board.Application.Actions;
using OffLogs.Api.Dto.Entities;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class GetOneActionTests: MyApiIntegrationTest
    {
        public GetOneActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(MainApiUrl.ApplicationGetOne)]
        public async Task OnlyAuthorizedUsersCanGetApplications(string url)
        {
            var user = await DataSeeder.CreateNewUser();
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
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

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
            var user1 = await DataSeeder.CreateNewUser();

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
    }
}