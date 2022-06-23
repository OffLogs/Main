using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.LogController
{
    public class LogShareActionTests : MyApiIntegrationTest
    {
        public LogShareActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory(Skip = "Skip is not used")]
        [InlineData(MainApiUrl.LogShare)]
        public async Task OnlyAuthorizedUsersCanDoIt(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new LogShareRequest()
            {
                Id = logs.First().Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        [Theory(Skip = "Skip is not used")]
        [InlineData(MainApiUrl.LogShare)]
        public async Task OtherUsersShouldNotShareLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var user2 = await DataSeeder.CreateActivatedUser();

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new LogShareRequest()
            {
                Id = logs.First().Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Theory(Skip = "Skip is not used")]
        [InlineData(MainApiUrl.LogShare)]
        public async Task OwnerShouldShareLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var log = logs.First();
            Assert.False(log.LogShares.Any());

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogShareRequest()
            {
                Id = log.Id
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var logShare = await response.GetJsonDataAsync<LogShareDto>();
            Assert.True(logShare.Id > 0);
            Assert.NotEmpty(logShare.Token);

            await DbSessionProvider.RefreshEntityAsync(log);
            Assert.True(log.LogShares.Any());
            Assert.Equal(log.LogShares.First().Token, logShare.Token);
        }

        [Theory(Skip = "Skip is not used")]
        [InlineData(MainApiUrl.LogShare)]
        public async Task UsersWithReadOnlyAccessCanDoIt(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            var user2 = await DataSeeder.CreateActivatedUser();

            await ApplicationService.ShareForUser(user.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new LogShareRequest()
            {
                Id = logs.First().Id
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var logShare = await response.GetJsonDataAsync<LogShareDto>();
            Assert.True(logShare.Id > 0);
            Assert.NotEmpty(logShare.Token);
        }
    }
}
