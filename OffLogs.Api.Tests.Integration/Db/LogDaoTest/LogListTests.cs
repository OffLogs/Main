using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;
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

            var log = await CreateLog(application, LogLevel.Error);
            Assert.True(log.Id > 0);
            Assert.NotEmpty(log.Properties);
            Assert.True(log.Properties.First().Id > 0);
            Assert.NotEmpty(log.Traces);
            Assert.True(log.Traces.First().Id > 0);
            
            var logFromDb = Dao.GetConnection().LoadSingleById<LogEntity>(log.Id);
            Assert.Equal(LogLevel.Error, logFromDb.Level);
            Assert.Equal(application.Id, logFromDb.ApplicationId);
        }
        
        [Fact]
        public async Task ShouldReceiveLogsList()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await CreateLog(application, LogLevel.Error);
            await CreateLog(application, LogLevel.Information);
            
            var (list, counter) = await LogDao.GetList(application.Id, 1, 30);
            Assert.Equal(2, counter);
            
            Assert.Contains(list, item => item.Level == LogLevel.Error);
            Assert.Contains(list, item => item.Level == LogLevel.Information);

            var firstLog = list.First();
            Assert.Single(firstLog.Properties);
            Assert.Single(firstLog.Traces);
        }

        private async Task<LogEntity> CreateLog(ApplicationEntity application, LogLevel level)
        {
            return await LogDao.AddAsync(
                application.Id, 
                "SomeMessage", 
                level, 
                DateTime.Now,
                new List<LogPropertyEntity>()
                {
                    new()
                    {
                        Key = "TEST_PROP",
                        Value = "TEST_VALUE",
                    }
                },
                new List<LogTraceEntity>()
                {
                    new()
                    {
                        Trace = "TestTrace",
                        CreateTime = DateTime.Now
                    }
                }
            );
        }
    }
}
