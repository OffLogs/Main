using OffLogs.Business.Notifications.Core.Emails;
using OffLogs.Business.Notifications.Core.Emails.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Tests.Integration.Core.Faker
{
    class FakeEmailSendingService : IEmailSendingService
    {
        private bool _isSent = false;

        public void Reset()
        {
            _isSent = false;
        }

        public string SendEmail(string to, EmailBuilder emailBuilder, string bcc)
        {
            return "";
        }

        public string SendEmail(string to, string subject, string body, string bcc)
        {
            return "";
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
            return "";
        }
    }
}
