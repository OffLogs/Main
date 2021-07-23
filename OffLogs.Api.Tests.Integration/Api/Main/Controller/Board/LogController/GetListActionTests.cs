using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Common.Constants;
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
            var response = await PostRequestAsAnonymousAsync(url, new GetListRequest()
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
            var response = await PostRequestAsync(url, user1.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user2.Applications.First().Id,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
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
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();
            Assert.Equal(1, responseData.TotalPages);
            Assert.Equal(3, responseData.Items.Count);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveMoreThanOnePages(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 25);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();
            Assert.Equal(2, responseData.TotalPages);
            Assert.Equal(GlobalConstants.ListPageSize, responseData.Items.Count);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveOrderedList(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs1 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var logs2 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();

            Assert.Equal(logs2.First().Id, responseData.Items.First().Id);
            Assert.Equal(logs1.First().Id, responseData.Items.Last().Id);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveOrderedListFilteredByLogLevel(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 3);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 7);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                LogLevel = LogLevel.Debug
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();
            Assert.Equal(7, responseData.Items.Count);
            foreach (var log in responseData.Items)
            {
                Assert.Equal(LogLevel.Debug, log.Level);
            }
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveCorrectIsFavoriteValue(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 3);            
            await LogService.SetIsFavoriteAsync(user.Id, logs.First().Id, true);
            await LogService.SetIsFavoriteAsync(user.Id, logs.Last().Id, true);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();
            Assert.True(responseData.Items.First().IsFavorite);
            Assert.True(responseData.Items.Last().IsFavorite);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveLogsForSharedApplications(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            await ApplicationService.ShareForUser(user2.Application, user);

            var logs1 = await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Information);
            var logs2 = await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Information);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user2.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();

            Assert.Equal(2, responseData.Items.Count);
            Assert.Contains(responseData.Items, l => l.Id == logs2.First().Id);
            Assert.Contains(responseData.Items, l => l.Id == logs1.First().Id);
        }
    }
}