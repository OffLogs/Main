using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api
{
    public class TooManyRecordsException : Exception, IDomainException
    {
        public TooManyRecordsException(string message = "Too many records") : base(message)
        {
        }
    }
}
