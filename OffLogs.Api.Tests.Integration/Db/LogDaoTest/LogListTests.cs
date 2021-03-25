using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Db.LogDaoTest
{
    [Collection("LogDaoTest.LogListTests")]
    public class LogListTests: MyIntegrationTest
    {
        public LogListTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Theory]
        [InlineData("some-user")]
        public async Task ShouldReceiveLogsList(string some)
        {
            await LogDao.GetList(1, 1);
        }
    }
}
