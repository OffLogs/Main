using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Board.LogController
{
    public class SetFavoriteActionTests: MyApiIntegrationTest
    {
        private const string Url = MainApiUrl.LogSetIsFavorite;
        
        public SetFavoriteActionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public async Task OnlyAuthorizedUsersCanSetAsFavorite()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            // Act
            var response = await PostRequestAsAnonymousAsync(Url, new SetIsFavoriteRequest()
            {
                LogId = logs.First().Id
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task OnlyOwnerCanSetAsFavorite()
        {
            var user1 = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user1.Applications.First().Id, LogLevel.Debug);
            
            var user2 = await DataSeeder.CreateActivatedUser();
            
            // Act
            var response = await PostRequestAsync(Url, user2.ApiToken, new SetIsFavoriteRequest()
            {
                LogId = logs.First().Id,
                IsFavorite = true
            });
            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task ShouldSetAsFavorite()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            var log = logs.First();
            Assert.False(await IsFavorite(log.Id));
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new SetIsFavoriteRequest()
            {
                IsFavorite = true,
                LogId = logs.First().Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            await DbSessionProvider.PerformCommitAsync();

            Assert.True(await IsFavorite(log.Id));
        }

        [Fact]
        public async Task ShouldUnSetAsFavorite()
        {
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.Applications.First().Id, LogLevel.Debug);
            
            var log = logs.First();
            
            Assert.False(await IsFavorite(log.Id));
            // Act
            var response = await PostRequestAsync(Url, user.ApiToken, new SetIsFavoriteRequest()
            {
                IsFavorite = false,
                LogId = logs.First().Id
            });
            response.EnsureSuccessStatusCode();
            // Assert
            await DbSessionProvider.PerformCommitAsync();

            Assert.True(!await IsFavorite(log.Id));
        }
        
        private async Task<bool> IsFavorite(long logId)
        {
            return (await QueryBuilder.FindByIdAsync<LogEntity>(logId)).IsFavorite;
        }
    }
}
