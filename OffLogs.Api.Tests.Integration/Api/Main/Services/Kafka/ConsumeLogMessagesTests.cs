using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Helpers;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ConsumeLogMessagesTests: MyApiIntegrationTest
    {
        public ConsumeLogMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendMessageAndReceiveIt()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(userModel.ApplicationApiToken, log1);
            
            // Receive 2 messages
            var processedRecords = await KafkaConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords > 0);

            var existsLog = await LogDao.GetLogAsync(log1.Token);
            Assert.NotNull(existsLog);
            Assert.Equal(log1.Message, existsLog.Message);
            Assert.Equal(log1.Level, existsLog.Level);
            Assert.Equal(log1.LogTime.ToLongDateString(), existsLog.LogTime.ToLongDateString());
            Assert.Equal(log1.Traces.Count, existsLog.Traces.Count);
            Assert.Equal(log1.Properties.Count, existsLog.Properties.Count);
        }
        
        [Fact]
        public async Task ShouldSendMessageAndReceiveItWithCancelationToken()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(userModel.ApplicationApiToken, log1);
            
            // Receive 2 messages
            var cancellationToken = new CancellationToken();
            var processedRecords = await KafkaConsumerService.ProcessLogsAsync(false, cancellationToken);
            Assert.True(processedRecords > 0);

            var existsLog = await LogDao.GetLogAsync(log1.Token);
            Assert.NotNull(existsLog);
        }
        
        [Fact]
        public async Task ShouldSendFewMessagesAndReceiveIt()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var logCounter = 10;
            var logs = new List<LogEntity>();
            // Create and send logs to Kafka
            for (int i = 1; i <= logCounter; i++)
            {
                var log = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
                logs.Add(log);
                await KafkaProducerService.ProduceLogMessageAsync(userModel.ApplicationApiToken, log);
            }

            // Receive 2 messages
            var processedRecords = await KafkaConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords >= logCounter);
            foreach (var expectedLog in logs)
            {
                Assert.True(await LogDao.IsLogExists(expectedLog.Token));
            }
        }
        
        [Fact]
        public async Task ShouldNotProcessLogsIfApplicationJwtIsIncorrect()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);

            var fakeJwt = SecurityUtil.GetTimeBasedToken();
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(fakeJwt, log1);
            
            // Receive 2 messages
            var processedRecords = await KafkaConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords > 0);

            var isExists = await LogDao.IsLogExists(log1.Token);
            Assert.False(isExists);

            var requestLog = await RequestLogDao.GetByTokenAsync(fakeJwt);
            Assert.NotNull(requestLog);
        }
    }
}
