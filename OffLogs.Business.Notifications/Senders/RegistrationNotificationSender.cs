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
    public class RegistrationNotificationSender : IAsyncNotification<RegistrationNotificationContext>
    {
        private readonly IEmailSendingService _emailSendingService;
        private readonly EmailFactory _emailFactory;

        public RegistrationNotificationSender(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
            _emailFactory = new EmailFactory();
        }

        public Task SendAsync(
            RegistrationNotificationContext commandContext, 
            CancellationToken cancellationToken = default
        )
        {
            var emailBuilder = _emailFactory.GetEmailBuilder("RegistrationNotification.htm");
            emailBuilder.AddPlaceholder("verificationToken", commandContext.VerificationUrl);
            _emailSendingService.SendEmail(commandContext.ToAddress, emailBuilder, null);
            return Task.CompletedTask;
        }
    }
}
