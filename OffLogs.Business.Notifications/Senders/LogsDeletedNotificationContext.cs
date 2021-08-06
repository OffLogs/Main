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
        public LogsDeletedNotificationContext(string to, int deletedLogsCount)
        {
            To = to;
            DeletedLogsCount = deletedLogsCount;
        }

        public string To { get; }
        public int DeletedLogsCount { get; }
    }
}
