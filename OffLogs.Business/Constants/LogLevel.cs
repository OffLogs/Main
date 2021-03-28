using OffLogs.Business.Extensions;

namespace OffLogs.Business.Constants
{
    public class LogLevel : AConstant<LogLevel>
    {
        public static readonly LogLevel Error = new LogLevel("E", "Error");
        public static readonly LogLevel Warning = new LogLevel("W", "Warning");
        public static readonly LogLevel Fatal = new LogLevel("F", "Fatal");
        public static readonly LogLevel Information = new LogLevel("I", "Information");
        public static readonly LogLevel Debug = new LogLevel("D", "Debug");
        
        public LogLevel() { }
        
        private LogLevel(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string value)
        {
            value = value.Trim().FirstCharToUpper();
            return value == Error.GetValue()
                   || value == Warning.GetValue()
                   || value == Fatal.GetValue()
                   || value == Information.GetValue()
                   || value == Debug.GetValue();
        }

        public override LogLevel FromString(string value)
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
    }
}