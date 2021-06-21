using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Services.Kafka
{
    [Collection("LogDaoTest.LogListTests")]
    public class LogListTests: MyIntegrationTest
    {
        public LogListTests(CustomWebApplicationFactory factory) : base(factory) {}

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
        }

        private async Task<LogEntity> CreateLog(ApplicationEntity application, LogLevel level)
        {
            return await LogDao.AddAsync(
                application, 
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
