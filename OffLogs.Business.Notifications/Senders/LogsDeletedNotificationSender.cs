using Notification.Abstractions;
using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Senders
{
    public class LogsDeletedNotificationSender : IAsyncNotification<LogsDeletedNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly EmailFactory _emailFactory;

        public LogsDeletedNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
            _emailFactory = new EmailFactory();
        }

        public Task SendAsync(LogsDeletedNotificationContext commandContext, CancellationToken cancellationToken = default)
        {
            var emailBuilder = _emailFactory.GetEmailBuilder("LogsDeletedNotification.htm");
            emailBuilder.AddPlaceholder("deletedLogsCount", commandContext.DeletedLogsCount.ToString());
            _emailSendingService.SendEmail(commandContext.To, emailBuilder, null);
            // Some sending logic
            return Task.CompletedTask;
        }
    }
}
