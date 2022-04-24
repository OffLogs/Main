using System.Collections.Generic;
using Notification.Abstractions;

namespace OffLogs.Business.Notifications.Senders.NotificationRule
{
    public class EmailNotificationContext : INotificationContext
    {
        public string Subject { get; set; }

        public string Body { get; set; }
        
        public ICollection<string> To { get; set; } = new List<string>();

        // For Newtonsoft
        public EmailNotificationContext() { }
    }
}
