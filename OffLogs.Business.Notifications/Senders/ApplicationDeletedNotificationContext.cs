using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class ApplicationDeletedNotificationContext : INotificationContext
    {
        // For Newtonsoft
        public ApplicationDeletedNotificationContext() { }

        public ApplicationDeletedNotificationContext(string to, string name)
        {
            Name = name;
            To = to;
        }

        public string Name { get; set; }
        public string To { get; set; }
    }
}
