using System;
using System.Net;

namespace OffLogs.Business.Common.Exceptions.Common
{
    public class HttpResponseException: Exception
    {
        public HttpStatusCode StatusCode { get; }
        
        public string Type { get; }

        public HttpResponseException(HttpStatusCode code, string message, string type = null): base(message)
        {
            StatusCode = code;
            Type = type;
        }
    }
}
