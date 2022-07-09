using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Notifications.Senders.NotificationRule;
using OffLogs.Business.Notifications.Senders.User;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Notifications.Emails.Emails
{
    public class EmailVerificationNotificationSenderTests : MyApiIntegrationTest
    {
        public EmailVerificationNotificationSenderTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldSendNotification()
        {
            var sentTo = "test@test.com";
            var url = "test@test.com";
            var actualTokenToken = "aaa+aaa/aaa";

            Assert.False(EmailSendingService.IsEmailSent);
            await NotificationBuilder.SendAsync(new EmailVerificationNotificationContext(
                sentTo,
                url,
                actualTokenToken
            ));

            Assert.True(EmailSendingService.IsEmailSent);
            
            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.To.Contains(sentTo)
                    && sentMessage.Body.Contains(url)
            );
        }
    }
}
