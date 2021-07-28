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
            var sendTo = "test123@test.com";
            var dto = new RegularLogsNotificationContext(sendTo, 3);

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);

            Assert.True(EmailSendingService.IsEmailSent);
            Assert.Equal(sendTo, EmailSendingService.SentTo);
            Assert.True(
                EmailSendingService.SubjectContains("Recent logs report")
            );
            Assert.True(
                EmailSendingService.BodyContains("logs were received recently")
            );
        }
    }
}
