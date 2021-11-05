using Notification.Abstractions;

namespace OffLogs.Business.Notifications.Senders
{
    public class RegistrationNotificationContext : INotificationContext
    {
        public RegistrationNotificationContext() {}

        public RegistrationNotificationContext(
            string toAddress, 
            string frontendUrl,
            string verificationToken
        )
        {
            ToAddress = toAddress;
            VerificationUrl = $"{frontendUrl}/registration/verification/{verificationToken}";
        }

        public string ToAddress { get; }
        public string VerificationUrl { get; }
    }
}
