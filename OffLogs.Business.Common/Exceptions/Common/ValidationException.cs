using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Common
{
    public class ValidationException : Exception, IDomainException
    {
        public ValidationException(): this("")
        {
        }

        public ValidationException(string message) : base(message)
        {
        }
    }
}
