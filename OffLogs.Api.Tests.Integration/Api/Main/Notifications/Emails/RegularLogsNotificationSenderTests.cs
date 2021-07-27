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
    public class RegularLogsNotificationSenderTests : MyApiIntegrationTest
    {
        public RegularLogsNotificationSenderTests(ApiCustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldSendNotification()
        {
            var userModel = await DataSeeder.CreateNewUser();
            var application = userModel.Applications.First();

            Assert.False(EmailSendingService.IsEmailSent);

            await NotificationBuilder.SendAsync(new RegularLogsNotificationContext());

            Assert.True(EmailSendingService.IsEmailSent);
        }
    }
}
