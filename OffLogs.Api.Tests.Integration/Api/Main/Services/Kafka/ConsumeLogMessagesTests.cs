using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ConsumeLogMessagesTests: MyApiIntegrationTest
    {
        public ConsumeLogMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendMessageAndReceiveIt()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(userModel.ApplicationApiToken, log1);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaLogsConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords > 0);

            var existsLog = await QueryBuilder.For<LogEntity>()
                .WithAsync(new LogGetByTokenCriteria(log1.Token));
            Assert.NotNull(existsLog);
            Assert.Equal(log1.EncryptedMessage, existsLog.EncryptedMessage);
            Assert.Equal(log1.Level, existsLog.Level);
            Assert.Equal(log1.LogTime.ToLongDateString(), existsLog.LogTime.ToLongDateString());
            Assert.Equal(log1.Traces.Count, existsLog.Traces.Count);
            Assert.Equal(log1.Properties.Count, existsLog.Properties.Count);
        }
        
        [Fact]
        public async Task ShouldSendMessageAndReceiveItWithCancellationToken()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);
            
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(userModel.ApplicationApiToken, log1);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var cancellationToken = new CancellationToken();
            var processedRecords = await KafkaLogsConsumerService.ProcessLogsAsync(false, cancellationToken);
            Assert.True(processedRecords > 0);

            var existsLog = await QueryBuilder.For<LogEntity>()
                .WithAsync(new LogGetByTokenCriteria(log1.Token), cancellationToken);
            Assert.NotNull(existsLog);
        }
        
        [Fact]
        public async Task ShouldSendFewMessagesAndReceiveIt()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
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
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaLogsConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords >= logCounter);
            foreach (var expectedLog in logs)
            {
                var isExists = await QueryBuilder.For<bool>()
                    .WithAsync(new LogIsExistsByTokenCriteria(expectedLog.Token));
                Assert.True(isExists);
            }
        }
        
        [Fact]
        public async Task ShouldNotProcessLogsIfApplicationJwtIsIncorrect()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            var log1 = await DataSeeder.MakeLogAsync(application, LogLevel.Error);

            var fakeJwt = SecurityUtil.GetTimeBasedToken();
            // Push 2 messages
            await KafkaProducerService.ProduceLogMessageAsync(fakeJwt, log1);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaLogsConsumerService.ProcessLogsAsync(false);
            Assert.True(processedRecords > 0);

            var isExists = await QueryBuilder.For<bool>()
                .WithAsync(new LogIsExistsByTokenCriteria(log1.Token));
            Assert.False(isExists);

            var requestLog = await QueryBuilder.For<RequestLogEntity>()
                .WithAsync(new RequestLogGetByTokenCriteria(fakeJwt));
            Assert.NotNull(requestLog);
        }
    }
}
