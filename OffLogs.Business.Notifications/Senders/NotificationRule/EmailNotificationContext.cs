using System.Collections.Generic;
using Notification.Abstractions;

namespace OffLogs.Business.Notifications.Senders.NotificationRule
{
    public class EmailNotificationContext : INotificationContext
    {
        public string Subject { get; set; }

        public string Body { get; set; }
        
        public string To { get; set; }

        // For Newtonsoft
        public EmailNotificationContext() { }
    }
}
