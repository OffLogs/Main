using OffLogs.Business.Common.Constants;
using OffLogs.Web.Constants;
using OffLogs.Web.Constants.Bootstrap;

namespace OffLogs.Web.Extensions
{
    public static class LogLevelExtensions
    {
        public static ColorType GetBootstrapColorType(this LogLevel level)
        {
            if (level == LogLevel.Error)
            {
                return ColorType.Danger;
            }
            if (level == LogLevel.Information)
            {
                return ColorType.Info;
            }
            if (level == LogLevel.Warning)
            {
                return ColorType.Warning;
            }
            if (level == LogLevel.Debug)
            {
                return ColorType.Secondary;
            }
            return ColorType.Secondary;
        }
    }
}