using System;
using System.Net;

namespace OffLogs.Business.Common.Exceptions
{
    public class HttpResponseStatusException: Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpResponseStatusException(HttpStatusCode code, string message): base(message)
        {
            StatusCode = code;
        }
    }
}