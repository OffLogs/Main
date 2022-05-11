namespace OffLogs.Web.Constants.Bootstrap
{
    public enum ColorType
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
        public static string GetName(this ColorType type)
        {
            if (type == ColorType.Primary)
                return "primary";
            if (type == ColorType.Secondary)
                return "secondary";
            if (type == ColorType.Success)
                return "success";
            if (type == ColorType.Danger)
                return "danger";
            if (type == ColorType.Warning)
                return "warning";
            if (type == ColorType.Info)
                return "info";
            if (type == ColorType.Dark)
                return "dark";
            if (type == ColorType.Light)
                return "light";
            return "";
        }
        
        public static string GetBadgeClass(this ColorType type)
        {
            if (
                type == ColorType.Warning
                || type == ColorType.Info
                || type == ColorType.Light
            )
                return type.GetName() + " text-dark";
            return type.GetName() + " text-white";
        }
    }
}
