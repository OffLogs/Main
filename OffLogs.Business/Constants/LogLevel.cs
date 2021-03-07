using OffLogs.Business.Extensions;

namespace OffLogs.Business.Constants
{
    public class LogLevel : AConstant<LogLevel>
    {
        public static readonly LogLevel Error = new LogLevel("Error", "Error");
        public static readonly LogLevel Warning = new LogLevel("Warning", "Warning");
        public static readonly LogLevel Fatal = new LogLevel("Fatal", "Fatal");
        public static readonly LogLevel Information = new LogLevel("Information", "Information");
        public LogLevel() { }
        
        private LogLevel(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string value)
        {
            value = value.Trim().FirstCharToUpper();
            return value == Error.ToString() 
                   || value == Warning.ToString() 
                   || value == Fatal.ToString() 
                   || value == Information.ToString();
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
            return null;
        }
    }
}