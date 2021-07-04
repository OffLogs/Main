using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Extensions;

namespace OffLogs.Business.Constants
{
    public class SiteLogLevel : AConstant<SiteLogLevel>
    {
        public static readonly SiteLogLevel Error = new SiteLogLevel("E", "Error");
        public static readonly SiteLogLevel Warning = new SiteLogLevel("W", "Warning");
        public static readonly SiteLogLevel Fatal = new SiteLogLevel("F", "Fatal");
        public static readonly SiteLogLevel Information = new SiteLogLevel("I", "Information");
        public static readonly SiteLogLevel Debug = new SiteLogLevel("D", "Debug");
        
        public SiteLogLevel() { }
        
        private SiteLogLevel(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string value)
        {
            value = value.Trim().FirstCharToUpper();
            return value == Error.GetValue()
                   || value == Warning.GetValue()
                   || value == Fatal.GetValue()
                   || value == Information.GetValue()
                   || value == Debug.GetValue();
        }

        public override SiteLogLevel FromString(string value)
        {
            value = value.Trim().FirstCharToUpper();
            if (Error.GetValue().Equals(value))
                return Error;
            if (Warning.GetValue().Equals(value))
                return Warning;
            if (Fatal.GetValue().Equals(value))
                return Fatal;
            if (Information.GetValue().Equals(value))
                return Information;
            if (Debug.GetValue().Equals(value))
                return Debug;
            return null;
        }
        
        public LogLevel ToLogLevel()
        {
            if (Equals(Error, this))
                return LogLevel.Error;
            if (Equals(Warning, this))
                return LogLevel.Warning;
            if (Equals(Fatal, this))
                return LogLevel.Fatal;
            if (Equals(Information, this))
                return LogLevel.Information;
            if (Equals(Debug, this))
                return LogLevel.Debug;
            return default;
        }
    }
}