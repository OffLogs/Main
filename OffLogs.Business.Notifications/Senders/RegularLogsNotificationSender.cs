using Notification.Abstractions;
using OffLogs.Business.Notifications.Core.Emails.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class RegularLogsNotificationSender : IAsyncNotification<RegularLogsNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;

        public RegularLogsNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
        }

        public Task SendAsync(
            RegularLogsNotificationContext commandContext, 
            CancellationToken cancellationToken = default
        )
        {
            // Some sending logic
            return Task.CompletedTask;
        }
    }
}
