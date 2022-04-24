namespace OffLogs.Business.Common.Constants
{
    public enum LogLevel
    {
        Error = 1,
        Warning = 2,
        Fatal = 3,
        Information = 4,
        Debug = 5
    }

    public static class LogLevelExtensions
    {
        public static string GetLabel(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Fatal:
                    return "Fatal";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Debug:
                    return "Debug";
            }
            return null;
        }
        
        public static string GetByName(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Fatal:
                    return "Fatal";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Debug:
                    return "Debug";
            }
            return null;
        }
    }
}
