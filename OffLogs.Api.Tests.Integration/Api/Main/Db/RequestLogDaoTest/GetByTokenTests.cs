using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.RequestLogDaoTest
{
    public class GetByTokenTests: MyApiIntegrationTest
    {
        public GetByTokenTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public async Task ShouldGetByToken()
        {
            var data = "{some data}";
            var clientIp = "127.0.0.1";
            var token = SecurityUtil.GetTimeBasedToken();
            
            var log = new RequestLogEntity(RequestLogType.Log, clientIp, "{some data}", token);
            await CommandBuilder.SaveAsync(log);
            
            var existsLog = await QueryBuilder.For<RequestLogEntity>()
                .WithAsync(new RequestLogGetByTokenCriteria(token));
            Assert.True(existsLog.Id > 0);
            Assert.NotNull(existsLog.CreateTime);
            Assert.Equal(RequestLogType.Log, existsLog.Type);
            Assert.Equal("\"{some data}\"", existsLog.Data);
            Assert.Equal(clientIp, existsLog.ClientIp);
        }
    }
}
