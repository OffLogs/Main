using Notification.Abstractions;
using OffLogs.Business.Helpers;
using OffLogs.Business.Notifications;
using OffLogs.Business.Notifications.Senders;
using System;
using Xunit;

namespace OffLogs.Api.Tests.Unit.Notifications
{
    public class NotificationContextExtensionsTests
    {
        private readonly string ContextTypeString = "OffLogs.Business.Notifications.Senders.RegularLogsNotificationContext";

        [Fact]
        public void ShouldReturnCorrectTypeAsString()
        {
            var context = new RegularLogsNotificationContext();

            var actualName = context.GetTypeAsString();
            Assert.Equal(ContextTypeString, actualName);
        }

        [Fact]
        public void ShouldReturnContextTypeByString()
        {
            var activationResult = Activator.CreateInstance(
                typeof(BusinessNotificationsAssemblyMarker).Assembly.GetName().Name, 
                ContextTypeString
            );
            var actualContext = (RegularLogsNotificationContext)activationResult.Unwrap();


            Assert.Equal(typeof(RegularLogsNotificationContext), actualContext.GetType());
        }
    }
}