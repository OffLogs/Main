using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Controller.Board.Application.Actions;
using OffLogs.Api.Dto.Entities;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class UpdateActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/application/update";
        
        public UpdateActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(Url)]
        public async Task OnlyAuthorizedUsersCanUpdateApplications(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new UpdateRequest()
            {
                Id = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanNotUpdateForOtherUser(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new UpdateRequest()
            {
                Id = user2.ApplicationId,
                Name = "SomeApp name"
            });
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanUpdateApplication(string url)
        {
            var applicationName = "NewApplicationName";
            var user1 = await DataSeeder.CreateNewUser();

            // Act
            Assert.NotEqual(applicationName, user1.Application.Name);
            var response = await PostRequestAsync(url, user1.ApiToken, new UpdateRequest()
            {
                Id = user1.ApplicationId,
                Name = applicationName
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.Equal(applicationName, responseData.Name);
        }
    }
}