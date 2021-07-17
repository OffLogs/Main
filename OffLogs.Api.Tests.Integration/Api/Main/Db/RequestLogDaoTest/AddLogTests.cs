using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.RequestLogDaoTest
{
    public class AddLogTests: MyApiIntegrationTest
    {
        public AddLogTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public async Task ShouldAddNewRequestLogWithLogType()
        {
            var log = await RequestLogDao.AddAsync(RequestLogType.Log, "127.0.0.1", "{some data}");
            Assert.True(log.Id > 0);
        }
        
        [Fact]
        public async Task ShouldAddNewRequestLogWithLogTypeAndObjectData()
        {
            var log = await RequestLogDao.AddAsync(RequestLogType.Log, "127.0.0.1", new { someData = 1123 });
            Assert.True(log.Id > 0);
        }
        
        [Fact]
        public async Task ShouldAddNewRequestLogWithRequestType()
        {
            var log = await RequestLogDao.AddAsync(RequestLogType.Request, "127.0.0.1", "{some data}");
            Assert.True(log.Id > 0);
        }
        
        [Fact]
        public async Task ShouldAddNewRequestLogWithRequestTypeAndObjectData()
        {
            var log = await RequestLogDao.AddAsync(RequestLogType.Request, "127.0.0.1", new { someData = 321 });
            Assert.True(log.Id > 0);
        }
    }
}
