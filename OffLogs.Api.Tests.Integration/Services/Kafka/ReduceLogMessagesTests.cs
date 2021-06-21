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
        public async Task ShouldSendSeveralMessagesToKafka()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log = await CreateLog(application, LogLevel.Error);
            for (int i = 0; i < 10; i++)
            {
                await KafkaService.ProduceLogMessageAsync(log);
            }

            var aaa = 123;
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
