using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Web.Constants
{
    public enum BootstrapColorType
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info
    }

    public static class BootstrapColorTypeExtensions
    {
        public static string GetName(this BootstrapColorType type)
        {
            if (type == BootstrapColorType.Primary)
                return "primary";
            if (type == BootstrapColorType.Secondary)
                return "secondary";
            if (type == BootstrapColorType.Success)
                return "success";
            if (type == BootstrapColorType.Danger)
                return "danger";
            if (type == BootstrapColorType.Warning)
                return "warning";
            if (type == BootstrapColorType.Info)
                return "info";
            return "";
        }
    }
}
