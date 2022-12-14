using System.Net;
using System.Web;
using Notification.Abstractions;

namespace OffLogs.Business.Notifications.Senders.User
{
    public class EmailVerificationNotificationContext : INotificationContext
    {
        public string ToAddress { get; set; }
        public string FrontendUrl { get; set; }
        public string VerificationToken { get; set; }
        public string VerificationUrl { get; set; }

        public EmailVerificationNotificationContext() {}

        public EmailVerificationNotificationContext(
            string toAddress, 
            string frontendUrl,
            string verificationToken    
        )
        {
            ToAddress = toAddress;
            FrontendUrl = frontendUrl;
            VerificationToken = WebUtility.UrlEncode(verificationToken);
            VerificationUrl = $"{FrontendUrl}/email/verification/" + HttpUtility.UrlPathEncode(VerificationToken);
        }
    }
}
