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
    public class UpdateActionTests: MyIntegrationTest
    {
        private const string Url = "/board/application/update";
        
        public UpdateActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData(Url)]
        public async Task OnlyAuthorizedUsersCanUpdateApplications(string url)
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
        public async Task CanNotUpdateForOtherUser(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new ApplicationUpdateModel()
            {
                Id = user2.ApplicationId,
                Name = "SomeApp name"
            });
            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [Theory]
        [InlineData(Url)]
        public async Task CanUpdateApplication(string url)
        {
            var applicationName = "NewApplicationName";
            var user1 = await DataSeeder.CreateNewUser();
            
            // Act
            Assert.NotEqual(applicationName, user1.Application.Name);
            var response = await PostRequestAsync(url, user1.ApiToken, new ApplicationUpdateModel()
            {
                Id = user1.ApplicationId,
                Name = applicationName
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationResponseModel>();
            Assert.Equal(applicationName, responseData.Data.Name);
        }
    }
}