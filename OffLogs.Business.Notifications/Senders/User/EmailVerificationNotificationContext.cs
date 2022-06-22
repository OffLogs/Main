﻿using System.Net;
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
            VerificationToken = verificationToken;
            VerificationUrl = $"{FrontendUrl}/email/verification/{VerificationToken}";
        }
    }
}
