using System;

namespace OffLogs.Web.Core.Exceptions
{
    public class ServerErrorException: Exception
    {
        public ServerErrorException(string message = "Connection error"): base(message)
        {
            
        }
    }
}