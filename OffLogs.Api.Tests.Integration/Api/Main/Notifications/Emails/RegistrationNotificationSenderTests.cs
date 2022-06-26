using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Notifications.Senders.User;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Notifications.Emails
{
    public class RegistrationNotificationSenderTests : MyApiIntegrationTest
    {
        public RegistrationNotificationSenderTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldSendNotification()
        {
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            Assert.False(EmailSendingService.IsEmailSent);
            var notificationContext = new RegistrationNotificationContext(
                userModel.Email,
                "https://font.url",
                userModel.VerificationToken
            );
            await NotificationBuilder.SendAsync(notificationContext);

            Assert.True(EmailSendingService.IsEmailSent);
            
            Assert.Contains(
                EmailSendingService.SentMessages,
                sentMessage => sentMessage.To.Contains(userModel.Email)
                    && sentMessage.Body.Contains(notificationContext.VerificationUrl)
            );
        }
    }
}
