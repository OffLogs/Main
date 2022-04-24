using System.Threading;
using System.Threading.Tasks;
using Notification.Abstractions;
using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Services;

namespace OffLogs.Business.Notifications.Senders.NotificationRule
{
    public class EmailNotificationSender : IAsyncNotification<EmailNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly EmailFactory _emailFactory;

        public EmailNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
            _emailFactory = new EmailFactory();
        }

        public Task SendAsync(EmailNotificationContext commandContext, CancellationToken cancellationToken = default)
        {
            var emailBuilder = _emailFactory.GetEmailBuilder("NotificationRuleNotification.htm");
            emailBuilder.Subject = commandContext.Subject;
            emailBuilder.AddPlaceholder("body", commandContext.Body);
            foreach (var to in commandContext.To)
            {
                _emailSendingService.SendEmail(to, emailBuilder, null);    
            }
            return Task.CompletedTask;
        }
    }
}
