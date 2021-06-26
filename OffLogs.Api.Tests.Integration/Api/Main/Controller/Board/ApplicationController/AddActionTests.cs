using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.ApplicationController
{
    public class AddActionTests: MyIntegrationTest
    {
        private const string Url = "/board/application/add";
        
        public AddActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData(Url)]
        public async Task OnlyAuthorizedUsersCanAddApplications(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new PaginatedRequestModel()
            {
                Page = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task CanAddApplication(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new ApplicationAddModel()
            {
                Name = "SomeApp name"
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationResponseModel>();
            Assert.True(responseData.Data.Id > 0);
            Assert.NotEmpty(responseData.Data.Name);
            Assert.Equal(user1.Id, responseData.Data.UserId);
        }
    }
}