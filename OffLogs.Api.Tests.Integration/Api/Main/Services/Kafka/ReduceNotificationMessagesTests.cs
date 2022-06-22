using System.Threading.Tasks;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Notifications.Senders.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ReduceNotificationMessagesTests : MyApiIntegrationTest
    {
        public ReduceNotificationMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendSeveralProduceNotificationMessageMessagesToKafka()
        {
            var notificationContext = new RegularLogsNotificationContext("test@test.com", 3);
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
        }

        [Fact]
        public async Task ShouldSendSeveralLogsDeletedNotificationMessagesToKafka()
        {
            var notificationContext = new LogsDeletedNotificationContext("test@test.com");
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
        }
        
        [Fact]
        public async Task ProduceRegistrationNotificationMessageToKafka()
        {
            var notificationContext = new RegistrationNotificationContext(
                "test@test.com",
                "https://font.url",
                "someToken"
            );
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
        }
    }
}
