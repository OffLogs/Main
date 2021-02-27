using System.Data;
using Vizit.Api.Mobile.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Common
{
    public class DbConnectionTests: MyIntegrationTest
    {
        public DbConnectionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public void ShouldTestDbConnection()
        {
            Assert.True(Dao.GetConnection().State == ConnectionState.Open);
        }
    }
}
