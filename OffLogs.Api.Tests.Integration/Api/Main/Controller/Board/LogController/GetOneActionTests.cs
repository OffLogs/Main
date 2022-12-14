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
    public class GetOneActionTests: MyApiIntegrationTest
    {
        public GetOneActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task OnlyAuthorizedUsersCanReceiveLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Information);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetRequest()
            {
                Id = logs.First().Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task ShouldReturnErrorIfLogNotFound(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetRequest()
            {
                Id = 0
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task OnlyOwnerCanReceiveLog(string url)
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            
            var user2 = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user2.ApplicationId, LogLevel.Information);
            var log = logs.First();
            
            // Act
            var response = await PostRequestAsync(url, user1.ApiToken, new GetRequest()
            {
                Id = log.Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task ShouldReceiveLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 1);
            var log = logs.First();
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetRequest()
            {
                Id = log.Id,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var receivedLog = await response.GetJsonDataAsync<LogDto>();
            Assert.NotNull(receivedLog);
            Assert.Equal(log.Properties.Count, receivedLog.Properties.Count);
            Assert.Equal(log.Traces.Count, receivedLog.Traces.Count);
            Assert.NotEmpty(log.EncryptedMessage);
            Assert.Equal(log.Id, receivedLog.Id);
            Assert.Equal(log.Level, receivedLog.Level);
            Assert.Equal(log.LogTime.ToLongTimeString(), receivedLog.LogTime.ToLongTimeString());
            Assert.Equal(log.LogTime.ToLongDateString(), receivedLog.LogTime.ToLongDateString());
        }

        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task ShouldReceiveLogFromSharedApplication(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var user2 = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 1);
            var log = logs.First();
            await ApplicationService.ShareForUser(user.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new GetRequest()
            {
                Id = log.Id,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var receivedLog = await response.GetJsonDataAsync<LogDto>();
            Assert.NotNull(receivedLog);
            Assert.Equal(log.Properties.Count, receivedLog.Properties.Count);
            Assert.Equal(log.Traces.Count, receivedLog.Traces.Count);
            Assert.NotEmpty(log.EncryptedMessage);
            Assert.Equal(log.Id, receivedLog.Id);
        }

        [Theory]
        [InlineData(MainApiUrl.LogGet)]
        public async Task ShouldReceiveSharedTokensWithLog(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 1);
            var log = logs.First();

            await LogShareService.Share(log);

            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetRequest()
            {
                Id = log.Id,
                PrivateKeyBase64 = user.PrivateKeyBase64
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var receivedLog = await response.GetJsonDataAsync<LogDto>();
            Assert.NotNull(receivedLog);
            Assert.NotEmpty(receivedLog.Shares);
            Assert.NotEmpty(receivedLog.Shares.First().Token);
        }
    }
}
