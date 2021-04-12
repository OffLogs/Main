using System;
using System.Net;

namespace OffLogs.Business.Common.Exceptions
{
    public class HttpResponseStatusException: Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }

        public HttpResponseStatusException(HttpStatusCode code, string message)
        {
            StatusCode = code;
            Message = message;
        }
    }
}