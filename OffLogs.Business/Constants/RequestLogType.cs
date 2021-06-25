using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Extensions;

namespace OffLogs.Business.Constants
{
    public class RequestLogType : AConstant<RequestLogType>
    {
        public static readonly RequestLogType Log = new RequestLogType("L", "Log");
        public static readonly RequestLogType Request = new RequestLogType("R", "Request");
        
        public RequestLogType() { }
        
        private RequestLogType(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string value)
        {
            value = value.Trim().FirstCharToUpper();
            return value == Log.GetValue()
                   || value == Request.GetValue();
        }

        public override RequestLogType FromString(string value)
        {
            value = value.Trim().FirstCharToUpper();
            if (Log.GetValue().Equals(value))
                return Log;
            if (Request.GetValue().Equals(value))
                return Request;
            return null;
        }
    }
}