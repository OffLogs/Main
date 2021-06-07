using System.Data;
using OffLogs.Api.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Common
{
    public class DbConnectionTests: MyIntegrationTest
    {
        public DbConnectionTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public void ShouldTestDbConnection()
        {
            Assert.True(Dao.IsConnectionSuccessful());
        }
    }
}
