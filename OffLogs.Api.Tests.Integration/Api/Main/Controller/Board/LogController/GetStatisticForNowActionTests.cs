using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Business.Controller.Board.Log.Actions;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.LogController
{
    public class GetStatisticForNowActionTests : MyApiIntegrationTest
    {
        public GetStatisticForNowActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Theory]
        [InlineData(MainApiUrl.LogGetStatisticForNow)]
        public async Task OnlyAuthorizedUsersCanReceiveData(string url)
        {
            var user = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetLogStatisticForNowRequest()
            {
                ApplicationId = user.Applications.First().Id,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(MainApiUrl.LogGetStatisticForNow)]
        public async Task OnlyOwnerCanReceiveApplications(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();

            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetLogStatisticForNowRequest()
            {
                ApplicationId = user2.Applications.First().Id,
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(MainApiUrl.LogGetStatisticForNow)]
        public async Task ShouldReceiveStatisticForOwnersApplication(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Error, 3);

            var user2 = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Error, 2);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetLogStatisticForNowRequest()
            {
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<LogStatisticForNowDto>();
            Assert.True(responseData.Count <= 2);
            Assert.Equal(
                3,
                responseData.Where(item => item.LogLevel == LogLevel.Error).Sum(item => item.Count)
            );
        }

        [Theory]
        [InlineData(MainApiUrl.LogGetStatisticForNow)]
        public async Task ShouldReceiveStatistic(string url)
        {
            var expectedCounter = 7;

            var user = await DataSeeder.CreateNewUser();
            await DataSeeder.CreateLogsAsync(user.Application.Id, LogLevel.Error, expectedCounter);
            await DataSeeder.CreateLogsAsync(user.Application.Id, LogLevel.Information, expectedCounter);
            await DataSeeder.CreateLogsAsync(user.Application.Id, LogLevel.Warning, expectedCounter);
            await DataSeeder.CreateLogsAsync(user.Application.Id, LogLevel.Fatal, expectedCounter);
            await DataSeeder.CreateLogsAsync(user.Application.Id, LogLevel.Debug, expectedCounter);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetListRequest()
            {
                Page = 1,
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<LogStatisticForNowDto>();
            var list = responseData;
            Assert.Equal(
                expectedCounter,
                list.Where(item => item.LogLevel == LogLevel.Error).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                list.Where(item => item.LogLevel == LogLevel.Information).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                list.Where(item => item.LogLevel == LogLevel.Warning).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                list.Where(item => item.LogLevel == LogLevel.Fatal).Sum(item => item.Count)
            );
            Assert.Equal(
                expectedCounter,
                list.Where(item => item.LogLevel == LogLevel.Debug).Sum(item => item.Count)
            );
        }
    }
}