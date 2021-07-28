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
            var dto = new RegularLogsNotificationContext();

            // Push 2 messages
            await KafkaProducerService.ProduceNotificationMessageAsync(dto);
            KafkaProducerService.Flush();
            
            // Receive 2 messages
            var processedRecords = await KafkaNotificationsConsumerService.ProcessNotificationsAsync(false);
            Assert.True(processedRecords > 0);
        }
    }
}
