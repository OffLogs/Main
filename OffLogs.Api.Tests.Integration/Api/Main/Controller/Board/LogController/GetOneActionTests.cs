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
    public class GetOneActionTests: MyApiIntegrationTest
    {
        public GetOneActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("/board/log/get")]
        public async Task OnlyAuthorizedUsersCanReceiveLog(string url)
        {
            var user = await DataSeeder.CreateNewUser();
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
        [InlineData("/board/log/get")]
        public async Task ShouldReturnErrorIfLogNotFound(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetRequest()
            {
                Id = 0
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData("/board/log/get")]
        public async Task OnlyOwnerCanReceiveLog(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            
            var user2 = await DataSeeder.CreateNewUser();
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
        [InlineData("/board/log/get")]
        public async Task ShouldReceiveLog(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 1);
            var log = logs.First();
            
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new GetRequest()
            {
                Id = log.Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var receivedLog = await response.GetJsonDataAsync<LogDto>();
            Assert.NotNull(receivedLog);
            Assert.Equal(log.Properties.Count, receivedLog.Properties.Count);
            Assert.Equal(log.Traces.Count, receivedLog.Traces.Count);
            Assert.Equal(log.Message, receivedLog.Message);
            Assert.Equal(log.Id, receivedLog.Id);
            Assert.Equal(log.Level, receivedLog.Level);
            Assert.Equal(log.LogTime.ToLongTimeString(), receivedLog.LogTime.ToLongTimeString());
            Assert.Equal(log.LogTime.ToLongDateString(), receivedLog.LogTime.ToLongDateString());
        }

        [Theory]
        [InlineData("/board/log/get")]
        public async Task ShouldReceiveLogFromSharedApplication(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var user2 = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug, 1);
            var log = logs.First();
            await ApplicationService.ShareForUser(user.Application, user2);

            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new GetRequest()
            {
                Id = log.Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var receivedLog = await response.GetJsonDataAsync<LogDto>();
            Assert.NotNull(receivedLog);
            Assert.Equal(log.Properties.Count, receivedLog.Properties.Count);
            Assert.Equal(log.Traces.Count, receivedLog.Traces.Count);
            Assert.Equal(log.Message, receivedLog.Message);
            Assert.Equal(log.Id, receivedLog.Id);
        }
    }
}