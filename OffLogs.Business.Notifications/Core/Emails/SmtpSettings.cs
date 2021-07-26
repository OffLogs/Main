using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Core.Emails
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string UserNameFrom { get; set; }
        public string EmailFrom { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
