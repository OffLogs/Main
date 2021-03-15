using OffLogs.Business.Extensions;

namespace OffLogs.Business.Constants
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
            return value == Error.ToString() 
                   || value == Warning.ToString() 
                   || value == Fatal.ToString() 
                   || value == Information.ToString()
                   || value == Debug.ToString();
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
    }
}