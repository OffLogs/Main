using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api
{
    public class ServerException : Exception, IDomainException
    {
        public ServerException(string message = "Server error") : base(message)
        {
        }
    }
}
