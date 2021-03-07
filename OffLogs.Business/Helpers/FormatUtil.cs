using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OffLogs.Business.Helpers
{
    public static class FormatUtil
    {
        public static string ClearUserName(string userName)
        {
            userName = (userName ?? "").Trim().ToLower();
            return Regex.Replace(userName, "[^A-Za-z0-9-_]", "");
        }
    }
}