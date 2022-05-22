using OffLogs.Business.Common.Constants;
using OffLogs.Web.Constants.Bootstrap;
using Radzen;

namespace OffLogs.Web.Core.Extensions
{
    public static class LogLevelExtensions
    {
        public static ColorType GetBootstrapColorType(this LogLevel level)
        {
            if (level == LogLevel.Error || level == LogLevel.Fatal)
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
        
        /**
         <RadzenBadge BadgeStyle="BadgeStyle.Primary" IsPill="true" Text="Primary"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Secondary" IsPill="true" Text="Secondary"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Light" IsPill="true" Text="Light"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Success" IsPill="true" Text="Success"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Danger" IsPill="true" Text="Danger"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Warning" IsPill="true" Text="Warning"/>
                    <RadzenBadge BadgeStyle="BadgeStyle.Info" IsPill="true" Text="Info"/> 
         */
        
        public static BadgeStyle GetBadgeStyle(this LogLevel level)
        {
            if (level == LogLevel.Error || level == LogLevel.Fatal)
            {
                return BadgeStyle.Danger;
            }
            if (level == LogLevel.Information)
            {
                return BadgeStyle.Info;
            }
            if (level == LogLevel.Warning)
            {
                return BadgeStyle.Warning;
            }
            if (level == LogLevel.Debug)
            {
                return BadgeStyle.Light;
            }
            return BadgeStyle.Secondary;
        }
    }
}
