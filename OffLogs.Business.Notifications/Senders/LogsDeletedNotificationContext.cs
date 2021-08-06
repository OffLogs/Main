using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class LogsDeletedNotificationContext : INotificationContext
    {
        public LogsDeletedNotificationContext(string to, DateTime? completitionTime = null)
        {
            To = to;
            CompletitionTime = completitionTime ?? DateTime.UtcNow;
        }

        public string To { get; }
        public DateTime CompletitionTime { get; }
    }
}
