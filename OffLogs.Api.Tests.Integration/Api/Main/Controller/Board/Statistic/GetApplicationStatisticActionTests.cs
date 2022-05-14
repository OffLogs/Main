using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Statistic;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.Statistic
{
    public class GetApplicationStatisticActionTests: MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.StatisticApplication;
        
        public GetApplicationStatisticActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task OnlyAuthorizedUsersCanReceiveList()
        {
            var user = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new GetApplicationStatisticRequest()
            {
                ApplicationId = user.ApplicationId
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ShouldReceiveStatistic()
        {
            var countErrors = 2;
            var countDebug = 3;
            var countFatal = 4;
            var countInformation = 5;
            var countWarning = 6;
            
            var user = await DataSeeder.CreateActivatedUser();
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Error, countErrors);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, countDebug);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Fatal, countFatal);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information, countInformation);
            await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Warning, countWarning);

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetApplicationStatisticRequest()
            {
                ApplicationId = user.ApplicationId
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var responseData = await response.GetJsonDataAsync<ApplicationStatisticDto>();
            Assert.Equal(
                countErrors + countDebug + countFatal + countInformation + countWarning, 
                responseData.LogsCount
            );
            Assert.Equal(countErrors, responseData.ErrorLogsCount);
            Assert.Equal(countDebug, responseData.DebugLogsCount);
            Assert.Equal(countFatal, responseData.FatalLogsCount);
            Assert.Equal(countInformation, responseData.InformationLogsCount);
            Assert.Equal(countWarning, responseData.WarningLogsCount);
            Assert.Equal(80, responseData.TracesCount);
            Assert.Equal(60, responseData.PropertiesCount);
        }
        
        [Fact]
        public async Task ShouldReceiveForOtherUser()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            
            await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Error, 3);

            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new GetApplicationStatisticRequest()
            {
                ApplicationId = user2.ApplicationId
            });
            
            // Assert
            var responseData = await response.GetDataAsStringAsync();
            Assert.Contains("DataPermissionException", responseData);
        }
    }
}
