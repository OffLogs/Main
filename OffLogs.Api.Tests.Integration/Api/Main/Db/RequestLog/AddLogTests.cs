using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.RequestLog
{
    public class AddLogTests : MyApiIntegrationTest
    {
        public AddLogTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldAddNewRequestLogWithLogType()
        {
            var log = new RequestLogEntity(RequestLogType.Log, "127.0.0.1", "{some data}");
            await CommandBuilder.SaveAsync(log);
            Assert.True(log.Id > 0);
        }

        [Fact]
        public async Task ShouldAddNewRequestLogWithLogTypeAndObjectData()
        {
            var log = new RequestLogEntity(RequestLogType.Log, "127.0.0.1", new { someData = 1123 });
            await CommandBuilder.SaveAsync(log);
            Assert.True(log.Id > 0);
        }

        [Fact]
        public async Task ShouldAddNewRequestLogWithRequestType()
        {
            var log = new RequestLogEntity(RequestLogType.Request, "127.0.0.1", "{some data}");
            await CommandBuilder.SaveAsync(log);
            Assert.True(log.Id > 0);
        }

        [Fact]
        public async Task ShouldAddNewRequestLogWithRequestTypeAndObjectData()
        {
            var log = new RequestLogEntity(RequestLogType.Request, "127.0.0.1", new { someData = 321 });
            await CommandBuilder.SaveAsync(log);
            Assert.True(log.Id > 0);
        }
    }
}
