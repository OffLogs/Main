using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.LogController
{
    public class GetListActionTests: MyApiIntegrationTest
    {
        public GetListActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task OnlyAuthorizedUsersCanReceiveList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.Applications.First().Id,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task OnlyOwnerCanReceiveApplications(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user2.Applications.First().Id,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveLogsList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Error, 3);
            
            var user2 = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Error, 2);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<LogResponseModel>>();
            Assert.Equal(1, responseData.Data.TotalPages);
            Assert.Equal(3, responseData.Data.Items.Count);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveMoreThanOnePages(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 25);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<LogResponseModel>>();
            Assert.Equal(2, responseData.Data.TotalPages);
            Assert.Equal(GlobalConstants.ListPageSize, responseData.Data.Items.Count);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveOrderedList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs1 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var logs2 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<LogResponseModel>>();
            
            Assert.Equal(logs2.First().Id, responseData.Data.Items.First().Id); 
            Assert.Equal(logs1.First().Id, responseData.Data.Items.Last().Id); 
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveOrderedListFilteredByLogLevel(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 3);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 7);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                LogLevel = LogLevel.Debug
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<LogResponseModel>>();
            Assert.Equal(7, responseData.Data.Items.Count);
            foreach (var log in responseData.Data.Items)
            {
                Assert.Equal(LogLevel.Debug, log.Level);
            } 
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveCorrectIsFavoriteValue(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var log = logs.First();
            await LogDao.SetIsFavoriteAsync(log.Id, true);
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogListRequestModel()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedResponseModel<LogResponseModel>>();
            Assert.True(responseData.Data.Items.First().IsFavorite);
        }
    }
}