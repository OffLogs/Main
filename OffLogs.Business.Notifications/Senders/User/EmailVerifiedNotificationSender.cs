using System.Threading;
using System.Threading.Tasks;
using Notification.Abstractions;
using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Services;

namespace OffLogs.Business.Notifications.Senders.User
{
    public class EmailVerifiedNotificationSender : IAsyncNotification<EmailVerifiedNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly EmailFactory _emailFactory;

        public EmailVerifiedNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
            _emailFactory = new EmailFactory();
        }

        public Task SendAsync(
            EmailVerifiedNotificationContext commandContext, 
            CancellationToken cancellationToken = default
        )
        {
            var emailBuilder = _emailFactory.GetEmailBuilder("UserEmailVerifiedNotification.htm");
            emailBuilder.AddPlaceholder("email", commandContext.VerifiedEmail);
            _emailSendingService.SendEmail(commandContext.ToAddress, emailBuilder, null);
            return Task.CompletedTask;
        }
    }
}
