using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class AddActionTests: MyApiIntegrationTest
    {
        private const string Url = "/board/application/add";
        
        public AddActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(Url)]
        public async Task OnlyAuthorizedUsersCanAddApplications(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new AddRequest()
            {
                Name = "aaa"
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(Url)]
        public async Task CanAddApplication(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new AddRequest()
            {
                Name = "SomeApp name"
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationDto>();
            Assert.True(responseData.Id > 0);
            Assert.NotEmpty(responseData.Name);
            Assert.Equal(user1.Id, responseData.UserId);
        }
    }
}