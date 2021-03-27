using System;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using Serilog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Db.LogDaoTest
{
    [Collection("LogDaoTest.LogListTests")]
    public class LogListTests: MyIntegrationTest
    {
        public LogListTests(CustomWebApplicationFactory factory) : base(factory) {}
        
        [Fact]
        public async Task ShouldAddNewErrorLog()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await LogDao.AddAsync(application.Id, "SomeMessage", LogLevel.Error, DateTime.Now);
        }
        
        [Fact]
        public async Task ShouldReceiveLogsList()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await LogDao.AddAsync(application.Id, "SomeMessage", LogLevel.Error, DateTime.Now);
            
            await LogDao.GetList(1, 1, 30);
        }
    }
}
