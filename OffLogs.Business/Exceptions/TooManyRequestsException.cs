using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Exceptions
{
    public class TooManyRequestsException : Exception, IDomainException
    {
        public TooManyRequestsException() : this("")
        {
        }

        public TooManyRequestsException(string entityName) : base($"Too many requests: {entityName}")
        {
        }
    }
}
