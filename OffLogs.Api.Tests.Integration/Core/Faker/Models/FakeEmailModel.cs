using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Tests.Integration.Core.Faker.Models
{
    public class FakeEmailModel
    {
        public FakeEmailModel(
            string from,
            string to,
            string subject,
            string body,
            string cc,
            string bcc
        )
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            Cc = cc;
            Bcc = bcc;
        }

        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }
        public string Cc { get; }
        public string Bcc { get; }
    }
}
