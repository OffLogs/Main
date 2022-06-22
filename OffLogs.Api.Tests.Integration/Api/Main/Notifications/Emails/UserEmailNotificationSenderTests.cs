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
    public class UserEmailNotificationSenderTests : MyApiIntegrationTest
    {
        public UserEmailNotificationSenderTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldSendNotification()
        {
            var userEmail = DataFactory.UserEmailFactory().Generate();
            var userModel = await DataSeeder.CreateActivatedUser();
            var application = userModel.Applications.First();

            Assert.False(EmailSendingService.IsEmailSent);
            var notificationContext = new EmailVerifiedNotificationContext(
                userModel.Email,
                userEmail.Email
            );
            await NotificationBuilder.SendAsync(notificationContext);

            Assert.True(EmailSendingService.IsEmailSent);
            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Equal(userModel.Email, sentMessage.To);
            Assert.Contains(userEmail.Email, sentMessage.Body);
        }
    }
}
