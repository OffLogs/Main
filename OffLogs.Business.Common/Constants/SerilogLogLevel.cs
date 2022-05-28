using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Extensions;

namespace OffLogs.Business.Common.Constants
{
    public class SerilogLogLevel : AConstant<SerilogLogLevel>
    {
        public static readonly SerilogLogLevel Error = new SerilogLogLevel("Error", "Error");
        public static readonly SerilogLogLevel Warning = new SerilogLogLevel("Warning", "Warning");
        public static readonly SerilogLogLevel Fatal = new SerilogLogLevel("Fatal", "Fatal");
        public static readonly SerilogLogLevel Information = new SerilogLogLevel("Information", "Information");
        public static readonly SerilogLogLevel Debug = new SerilogLogLevel("Debug", "Debug");
        public SerilogLogLevel() { }
        
        private SerilogLogLevel(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string value)
        {
            value = value.Trim().FirstCharToUpper();
            return value == Error.GetValue() 
                   || value == Warning.GetValue() 
                   || value == Fatal.GetValue() 
                   || value == Information.GetValue()
                   || value == Debug.GetValue();
        }

        public override SerilogLogLevel FromString(string Value)
        {
            if (Error.GetValue().Equals(Value))
                return Error;
            if (Warning.GetValue().Equals(Value))
                return Warning;
            if (Fatal.GetValue().Equals(Value))
                return Fatal;
            if (Information.GetValue().Equals(Value))
                return Information;
            if (Debug.GetValue().Equals(Value))
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
