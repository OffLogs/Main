using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Db.LogDaoTest
{
    [Collection("LogDaoTest.LogListTests")]
    public class LogListTests: MyApiIntegrationTest
    {
        public LogListTests(ApiCustomWebApplicationFactory factory) : base(factory) {}
        
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
            
            var logFromDb = await QueryBuilder.FindByIdAsync<LogEntity>(log.Id);
            Assert.Equal(LogLevel.Error, logFromDb.Level);
            Assert.Equal(application.Id, logFromDb.Application.Id);
        }
        
        [Fact]
        public async Task ShouldReceiveLogsList()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            await CreateLog(application, LogLevel.Error);
            await CreateLog(application, LogLevel.Information);
            
            var list = await GetLogsList(application.Id);
            Assert.Equal(2, list.TotalCount);
            
            Assert.Contains(list.Items, item => item.Level == LogLevel.Error);
            Assert.Contains(list.Items, item => item.Level == LogLevel.Information);
        }

        private async Task<LogEntity> CreateLog(ApplicationEntity application, LogLevel level)
        {
            return await LogService.AddAsync(
                application, 
                "SomeMessage", 
                level,
                DateTime.UtcNow,
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
                        CreateTime = DateTime.UtcNow
                    }
                }
            );
        }
    }
}
