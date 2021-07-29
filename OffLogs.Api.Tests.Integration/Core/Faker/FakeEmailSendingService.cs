using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Tests.Integration.Core.Faker
{
    public class FakeEmailSendingService : IEmailSendingService
    {
        public bool IsEmailSent = false;
        public string SentBody = "";
        public string SentSubject = "";
        public string SentTo = "";

        public void Reset()
        {
            IsEmailSent = false;
            SentBody = "";
            SentSubject = "";
            SentTo = "";
        }

        public string SendEmail(string to, EmailBuilder emailBuilder, string bcc)
        {
            emailBuilder.Build();
            return SendEmail("", to, emailBuilder.Subject, emailBuilder.Body, null, bcc);
        }

        public string SendEmail(string to, string subject, string body, string bcc)
        {
            return SendEmail("", to, subject, body, null, bcc);
        }

        public string SendEmail(
            string from, 
            string to, 
            string subject, 
            string body, 
            string cc, 
            string bcc
        )
        {
            IsEmailSent = true;
            SentBody = body;
            SentSubject = subject;
            SentTo = to;
            return "";
        }
    }
}
