using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Core.Emails
{
    class EmailTemplateModel
    {
        // this class is stored in a template cache

        public string BodyTemplate { get; set; } // already includes the LAYOUT TEMPL + CONTENT TEMPL, both still have {placeholders}
        public string SubjectTemplate { get; set; } // subject is extracted from CONTENT TEMPL <subject> tag
    }
}
