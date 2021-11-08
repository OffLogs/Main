using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Test.Extensions;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Controller.Public.LogController
{
    public class GetBySharedTokenActionTests : MyApiIntegrationTest
    {
        public GetBySharedTokenActionTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Theory]
        [InlineData(MainApiUrl.LogGetSharedByToken)]
        public async Task ShouldNotLoginIfNotExists(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetBySharedTokenRequest
            {
                Token = "test_not_exists",
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(MainApiUrl.LogGetSharedByToken)]
        public async Task ShouldNotLoginIfEmpty(string url)
        {
            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetBySharedTokenRequest
            {
                Token = null,
            });
            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(MainApiUrl.LogGetSharedByToken)]
        public async Task ShouldNotLoginIfPasswordIsIncorrect(string url)
        {
            // Arrange
            var user = await DataSeeder.CreateActivatedUser();
            var logs = await DataSeeder.CreateLogsAsync(user.ApplicationId, LogLevel.Debug);
            var log = logs.First();
            var logShare = await LogShareService.Share(log);

            // Act
            var response = await PostRequestAsAnonymousAsync(url, new GetBySharedTokenRequest
            {
                Token = logShare.Token
            });
            // Assert
            response.EnsureSuccessStatusCode();
            var actualLog = await response.GetJsonDataAsync<LogSharedDto>();
            Assert.Equal(log.Id, actualLog.Id);
            Assert.Equal(log.Level, actualLog.Level);
            Assert.NotEmpty(log.EncryptedMessage);
            Assert.Equal(log.Properties.Count, actualLog.Properties.Count);
            Assert.Equal(log.Traces.Count, actualLog.Traces.Count);
        }
    }
}
