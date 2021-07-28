using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class RegularLogsNotificationContext : INotificationContext
    {
        public RegularLogsNotificationContext() {}

        public RegularLogsNotificationContext(
            string to,
            int errorCounter
        )
        {
            To = to;
            ErrorCounter = errorCounter;
        }

        public string To { get; set; }
        public int ErrorCounter { get; set; }
    }
}
