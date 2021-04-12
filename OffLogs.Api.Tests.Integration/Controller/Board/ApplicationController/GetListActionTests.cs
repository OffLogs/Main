using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Models.Request;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Models.Response.Board;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Controller.Board.ApplicationController
{
    public class GetListActionTests: MyIntegrationTest
    {
        public GetListActionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("/board/application/list")]
        public async Task OnlyAuthorizedUsersCanReceiveList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new PaginatedRequestModel()
            {
                Page = 1
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData("/board/application/list")]
        public async Task OnlyOwnerCanReceiveApplications(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new PaginatedRequestModel()
            {
                Page = 1
            });
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<ApplicationResponseModel>>();
            Assert.Single(responseData.Data.Items);
            Assert.Equal(user1.Id, responseData.Data.Items.First().UserId);
        }
        
        [Theory]
        [InlineData("/board/application/list")]
        public async Task ShouldReceiveLogsList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user.Id, 3);
            
            var user2 = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user2.Id, 2);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new PaginatedRequestModel()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<ApplicationResponseModel>>();
            Assert.Equal(1, responseData.Data.TotalPages);
            Assert.Equal(4, responseData.Data.Items.Count);
        }
        
        [Theory]
        [InlineData("/board/application/list")]
        public async Task ShouldReceiveMoreThanOnePages(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateApplicationsAsync(user.Id, 25);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new PaginatedRequestModel()
            {
                Page = 1
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<ApplicationResponseModel>>();
            Assert.Equal(2, responseData.Data.TotalPages);
            Assert.Equal(26, responseData.Data.Items.Count);
        }
    }
}