using System;
using Microsoft.VisualBasic;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Constants;

namespace OffLogs.Web.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFullDateTime(this DateTime time)
        {
            return time.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
}