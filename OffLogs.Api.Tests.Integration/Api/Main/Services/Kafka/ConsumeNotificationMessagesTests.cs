using System.Threading.Tasks;
using OffLogs.Business.Notifications.Senders;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ConsumeNotificationMessagesTests : MyApiIntegrationTest
    {
        public ConsumeNotificationMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendMessageAndReceiveIt()
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
            Assert.Contains("Recent logs report", EmailSendingService.SentSubject);
            Assert.Contains("logs were received recently", EmailSendingService.SentBody);
            Assert.Contains(toAddress, EmailSendingService.SentTo);
        }
    }
}
