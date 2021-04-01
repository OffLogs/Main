using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Models.Request;
using OffLogs.Api.Models.Request.Board;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Models.Response.Board;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller.Board.ApplicationController
{
    public class AddActionTests: MyIntegrationTest
    {
        private const string Url = "/board/application/add";
        
        public AddActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
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
        public async Task OnlyOwnerCanReceiveApplications(string url)
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