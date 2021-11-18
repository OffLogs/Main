using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Queries.Entities.Log;
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
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new SetIsFavoriteRequest()
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
            var user1 = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user1.Applications.First().Id, LogLevel.Debug);
            
            var user2 = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsync(url, user2.ApiToken, new SetIsFavoriteRequest()
            {
                LogId = logs.First().Id,
                IsFavorite = true
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [Theory]
        [InlineData("/board/log/setFavorite")]
        public async Task ShouldSetAsFavorite(string url)
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            var log = logs.First();
            Assert.False(await IsFavorite(user.Id, log.Id));
            // Act
            var response = await PostRequestAsync(url, user.ApiToken, new SetIsFavoriteRequest()
            {
                IsFavorite = true,
                LogId = logs.First().Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            await DbSessionProvider.PerformCommitAsync();

            Assert.True(await IsFavorite(user.Id, log.Id));
        }

        private async Task<bool> IsFavorite(long userId, long logId)
        {
            return await QueryBuilder.For<bool>().WithAsync(
                new LogIsFavoriteCriteria(userId, logId)
            );
        }
    }
}