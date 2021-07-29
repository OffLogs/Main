using System.Threading.Tasks;
using OffLogs.Business.Notifications.Senders;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Services.Kafka
{
    public class ReduceNotificationMessagesTests : MyApiIntegrationTest
    {
        public ReduceNotificationMessagesTests(ApiCustomWebApplicationFactory factory) : base(factory) {}

        [Fact]
        public async Task ShouldSendSeveralMessagesToKafka()
        {
            var notificationContext = new RegularLogsNotificationContext();
            await KafkaProducerService.ProduceNotificationMessageAsync(notificationContext);
        }
    }
}
