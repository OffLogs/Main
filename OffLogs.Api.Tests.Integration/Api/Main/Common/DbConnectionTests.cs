using OffLogs.Api.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Common
{
    public class DbConnectionTests: MyApiIntegrationTest
    {
        public DbConnectionTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public void ShouldTestDbConnection()
        {
            Assert.True(DbSessionProvider.CurrentSession.IsConnected);
        }
    }
}
