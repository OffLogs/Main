using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Notifications.Core.Utils
{
    class LocalizationUtils
    {
        #region Localization
        public static bool IsEnglish()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name.StartsWith("en");
        }

        public static string CultureCode()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2);
        }
        #endregion
    }
}
