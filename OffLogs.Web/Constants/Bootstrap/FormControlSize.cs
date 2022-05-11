namespace OffLogs.Web.Constants.Bootstrap;

public enum FormControlSize
{
    Large = 1,
    Medium,
    Small
}

public static class FormControlSizeExtensions
{
    public static string GetSizeClass(this FormControlSize type)
    {
        if (type == FormControlSize.Large)
            return "form-control-lg";
        if (type == FormControlSize.Small)
            return "form-control-sm";
        return "";
    }
}
