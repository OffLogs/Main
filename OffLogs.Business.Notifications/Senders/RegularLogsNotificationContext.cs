using Notification.Abstractions;

namespace OffLogs.Business.Notifications.Senders
{
    public class RegularLogsNotificationContext : INotificationContext
    {
        public RegularLogsNotificationContext() {}

        public RegularLogsNotificationContext(string toAddress, int errorCounter)
        {
            ToAddress = toAddress;
            ErrorCounter = errorCounter;
        }

        public string ToAddress { get; set; }
        public int ErrorCounter { get; set; }
    }
}
