using System;
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
            var user = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.Applications.First().Id,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task OnlyOwnerCanReceiveApplications(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user2.Applications.First().Id,
                PrivateKeyBase64 = user2.PrivateKeyBase64
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveLogsList(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Error, 3);

            var user2 = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Error, 2);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                PrivateKeyBase64 = user.PrivateKeyBase64
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
            var totalLogs = GlobalConstants.ListPageSize * 2 + 1;
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateLogsAsync(
                user.ApplicationId,
                LogLevel.Information,
                totalLogs
            );

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();
            Assert.Equal(3, responseData.TotalPages);
            Assert.Equal(totalLogs - 1, responseData.Items.Count);
        }

        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldReceiveOrderedList(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs1 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var logs2 = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                PrivateKeyBase64 = user.PrivateKeyBase64
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
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 3);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 7);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                LogLevel = LogLevel.Debug,
                PrivateKeyBase64 = user.PrivateKeyBase64
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
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, 3);            
            await LogService.SetIsFavoriteAsync(user.Id, logs.First().Id, true);
            await LogService.SetIsFavoriteAsync(user.Id, logs.Last().Id, true);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                PrivateKeyBase64 = user.PrivateKeyBase64
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
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user2.Application, user);

            var logs1 = await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Information);
            var logs2 = await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Information);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user2.ApplicationId,
                PrivateKeyBase64 = user2.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();

            Assert.Equal(2, responseData.Items.Count);
            Assert.Contains(responseData.Items, l => l.Id == logs2.First().Id);
            Assert.Contains(responseData.Items, l => l.Id == logs1.First().Id);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogList)]
        public async Task ShouldCreateSeveralLogsAndReceiveEncryptedLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var log1 = (await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information)).First();
            var log2 = (await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Warning)).First();
            var log3 = (await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Error)).First();

            log1 = await LogAssembler.AssembleDecryptedLogAsync(
                log1, 
                Convert.FromBase64String(user.PrivateKeyBase64)
            );
            log2 = await LogAssembler.AssembleDecryptedLogAsync(
                log2, 
                Convert.FromBase64String(user.PrivateKeyBase64)
            );
            log3 = await LogAssembler.AssembleDecryptedLogAsync(
                log3, 
                Convert.FromBase64String(user.PrivateKeyBase64)
            );
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<PaginatedListDto<LogListItemDto>>();

            Assert.Contains(responseData.Items, item =>
            {
                return item.Id == log1.Id
                       && item.Message == log1.Message;
            });
            Assert.Contains(responseData.Items, item =>
            {
                return item.Id == log2.Id
                       && item.Message == log2.Message;
            });
            Assert.Contains(responseData.Items, item =>
            {
                return item.Id == log3.Id
                       && item.Message == log3.Message;
            });
        }
    }
}
