using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Exceptions
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
