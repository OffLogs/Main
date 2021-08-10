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
    public class ApplicationDeletedNotificationSender : IAsyncNotification<ApplicationDeletedNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly EmailFactory _emailFactory;

        public ApplicationDeletedNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
            _emailFactory = new EmailFactory();
        }

        public Task SendAsync(ApplicationDeletedNotificationContext commandContext, CancellationToken cancellationToken = default)
        {
            var emailBuilder = _emailFactory.GetEmailBuilder("ApplicationDeletedNotification.htm");
            emailBuilder.AddPlaceholder("{{name}}", commandContext.Name);
            _emailSendingService.SendEmail(commandContext.To, emailBuilder, null);
            return Task.CompletedTask;
        }
    }
}
