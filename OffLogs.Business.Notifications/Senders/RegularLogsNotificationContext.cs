﻿using Notification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class RegularLogsNotificationContext : INotificationContext
    {
        public RegularLogsNotificationContext(int errorCounter = 0)
        {
            ErrorCounter = errorCounter;
        }

        public int ErrorCounter { get; }
    }
}
