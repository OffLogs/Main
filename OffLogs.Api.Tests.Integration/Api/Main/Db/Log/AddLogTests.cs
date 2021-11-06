using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.Log
{
    [Collection("LogDaoTest.AddLogTests")]
    public class AddLogTests : MyApiIntegrationTest
    {
        public AddLogTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldAddNewLogsWithUniqToken()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            var logs = new List<LogEntity>();
            Parallel.For(0, 100, async (index) =>
            {
                logs.Add(
                    await DataSeeder.MakeLogAsync(application, LogLevel.Error)
                 );
            });
            Assert.True(
                logs.DistinctBy(log => log.Token).Count() == logs.Count()
            );
        }

        [Fact]
        public async Task ShouldAddNewLogWithUniqToken()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            var logs = await DataSeeder.CreateLogsAsync(application.Id, LogLevel.Error, 10);
            Assert.True(
                logs.DistinctBy(log => log.Token).Count() == logs.Count()
            );
        }
    }
}
