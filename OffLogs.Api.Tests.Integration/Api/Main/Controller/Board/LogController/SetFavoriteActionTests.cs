using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Criteria;
using OffLogs.Business.Orm.Entities;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.LogController
{
    public class SetFavoriteActionTests: MyApiIntegrationTest
    {
        public SetFavoriteActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("/board/log/setFavorite")]
        public async Task OnlyAuthorizedUsersCanSetAsFavorite(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new LogSetFavoriteRequestModel()
            {
                LogId = logs.First().Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData("/board/log/setFavorite")]
        public async Task OnlyOwnerCanSetAsFavorite(string url)
        {
            var user1 = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user1.Applications.First().Id, LogLevel.Debug);
            
            var user2 = await DataSeeder.CreateNewUser();
            
            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new LogSetFavoriteRequestModel()
            {
                LogId = logs.First().Id,
                IsFavorite = true
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden);
        }
        
        [Theory]
        [InlineData("/board/log/setFavorite")]
        public async Task ShouldSetAsFavorite(string url)
        {
            var user = await DataSeeder.CreateNewUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);


            var log = logs.First();
            Assert.False(log.IsFavorite);
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new LogSetFavoriteRequestModel()
            {
                IsFavorite = true,
                LogId = logs.First().Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            var actualLog = await QueryBuilder.FindByIdAsync<LogEntity>(log.Id);
            Assert.True(actualLog.IsFavorite);
        }
    }
}