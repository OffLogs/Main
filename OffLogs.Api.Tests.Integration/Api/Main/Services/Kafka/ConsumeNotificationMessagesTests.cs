using System;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Notifications.Senders;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ConsumeNotificationMessagesTests : MyApiIntegrationTest
    {
        public ConsumeNotificationMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendRegularLogsNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var dto = new RegularLogsNotificationContext()
            { 
                ToAddress = toAddress,
                ErrorCounter = 3
            };

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);

            Assert.True(EmailSendingService.IsEmailSent);
            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Contains("Recent logs report", sentMessage.Subject);
            Assert.Contains("logs were received recently", sentMessage.Body);
            Assert.Contains(toAddress, sentMessage.To);
        }

        [Fact]
        public async Task ShouldSendRegularLogsDeletedNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var sentDate = DateTime.Now;
            var dto = new LogsDeletedNotificationContext(toAddress, sentDate);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Contains("Logs deletion notification", sentMessage.Subject);
            Assert.Contains(sentDate.ToString("G"), sentMessage.Body);
            Assert.Contains(toAddress, sentMessage.To);
        }

        [Fact]
        public async Task ShouldSendRegularApplicationDeletedNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var applicationName = "TestAppName";
            var dto = new ApplicationDeletedNotificationContext(toAddress, applicationName);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Contains("Application has been deleted", sentMessage.Subject);
            Assert.Contains(applicationName, sentMessage.Body);
            Assert.Contains(toAddress, sentMessage.To);
        }
        
        [Fact]
        public async Task ShouldSendRegistrationNotificationAndReceiveIt()
        {
            var toAddress = "test123@test.com";
            var frontUrl = "https://font.url";
            var token = "someToken";
            var notificationContext = new RegistrationNotificationContext(toAddress, frontUrl, token);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
            KafkaProducerService.Flush();

            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
            Assert.True(EmailSendingService.IsEmailSent);

            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Contains("OffLogs verification", sentMessage.Subject);
            Assert.Contains(notificationContext.VerificationUrl, sentMessage.Body);
            Assert.Contains(toAddress, sentMessage.To);
        }
    }
}
