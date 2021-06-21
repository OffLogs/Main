using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

            var log = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            for (int i = 0; i < 10; i++)
            {
                await KafkaProducerService.ProduceLogMessageAsync(log);
            }
        }
        
        [Fact]
        public async Task ShouldSendMessageAndReceiveIt()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            await KafkaProducerService.ProduceLogMessageAsync(log);
            await KafkaConsumerService.ProcessLogsAsync();
        }
    }
}
