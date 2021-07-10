namespace OffLogs.Web.Shared.Ui.Components.Badges
{
    public enum BadgeColor
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
    
    public static class BadgeColorExtensions
    {
        public static string GetClass(this BadgeColor type)
        {
            if (type == BadgeColor.Primary)
                return "primary";
            if (type == BadgeColor.Secondary)
                return "secondary";
            if (type == BadgeColor.Success)
                return "success";
            if (type == BadgeColor.Danger)
                return "danger";
            if (type == BadgeColor.Warning)
                return "warning text-dark";
            if (type == BadgeColor.Info)
                return "info text-dark";
            if (type == BadgeColor.Dark)
                return "dark";
            if (type == BadgeColor.Light)
                return "info text-dark";
            return "";
        }
    }
}