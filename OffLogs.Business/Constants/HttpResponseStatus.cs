using System;

namespace OffLogs.Business.Constants
{
    public class HttpResponseStatus : AConstant<HttpResponseStatus>
    {
        public static readonly HttpResponseStatus Ok = new HttpResponseStatus("ok", "ok");
        public static readonly HttpResponseStatus Fail = new HttpResponseStatus("fail", "fail");
        
        public HttpResponseStatus() { }
        
        private HttpResponseStatus(string value, string name) : base(value, name) { }
        
        public override bool IsValid(string Value)
        {
            var value = Value.Trim().ToLower();
            return value == Ok.ToString() || value == Fail.ToString();
        }

        public override HttpResponseStatus FromString(string Value)
        {
            throw new NotImplementedException();
        }
    }
}