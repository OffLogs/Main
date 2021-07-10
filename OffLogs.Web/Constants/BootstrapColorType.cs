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
        Info,
        Light,
        Dark
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
            if (type == BootstrapColorType.Dark)
                return "dark";
            if (type == BootstrapColorType.Light)
                return "light";
            return "";
        }
        
        public static string GetBadgeClass(this BootstrapColorType type)
        {
            if (
                type == BootstrapColorType.Warning
                || type == BootstrapColorType.Info
                || type == BootstrapColorType.Light
            )
                return type.GetName() + " text-dark";
            return type.GetName();
        }
    }
}
