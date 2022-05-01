using OffLogs.Business.Common.Constants;
using OffLogs.Web.Constants;

namespace OffLogs.Web.Extensions
{
    public static class LogLevelExtensions
    {
        public static BootstrapColorType GetBootstrapColorType(this LogLevel level)
        {
            if (level == LogLevel.Error)
            {
                return BootstrapColorType.Danger;
            }
            if (level == LogLevel.Information)
            {
                return BootstrapColorType.Info;
            }
            if (level == LogLevel.Warning)
            {
                return BootstrapColorType.Warning;
            }
            if (level == LogLevel.Debug)
            {
                return BootstrapColorType.Secondary;
            }
            return BootstrapColorType.Secondary;
        }
    }
}