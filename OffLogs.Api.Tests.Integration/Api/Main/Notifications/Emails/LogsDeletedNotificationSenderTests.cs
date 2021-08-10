using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Notifications.Senders;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Orm.Queries.Entities.RequestLog;
using Xunit;

namespace OffLogs.Api.Tests.Integration.Api.Main.Notifications.Emails
{
    public class LogsDeletedNotificationSenderTests : MyApiIntegrationTest
    {
        public LogsDeletedNotificationSenderTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldSendNotification()
        {
            var sentTo = "test@test.com";
            var expectedTime = DateTime.UtcNow;

            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            Assert.False(EmailSendingService.IsEmailSent);
            await NotificationBuilder.SendAsync(new LogsDeletedNotificationContext(sentTo, expectedTime));

            Assert.True(EmailSendingService.IsEmailSent);
            var sentMessage = EmailSendingService.SentMessages.First();
            Assert.Equal(sentTo, sentMessage.To);
            Assert.Contains(expectedTime.ToString("G"), sentMessage.Body);
        }
    }
}
