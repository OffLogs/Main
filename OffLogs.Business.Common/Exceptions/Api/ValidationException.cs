using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api
{
    public class ValidationException : Exception, IDomainException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
