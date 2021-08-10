using OffLogs.Api.Tests.Integration.Core.Faker.Models;
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
        public List<FakeEmailModel> SentMessages = new ();
        public bool IsEmailSent
        {
            get => SentMessages.Any();
        }
        
        public void Reset()
        {
            SentMessages = new ();
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
            SentMessages.Add(new FakeEmailModel(from, to, subject, body, cc, bcc));
            return "";
        }
    }
}
