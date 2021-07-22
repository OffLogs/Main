using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Exceptions
{
    public class PermissionException : Exception, IDomainException
    {
        public PermissionException(): this("")
        {
        }

        public PermissionException(string message) : base(message)
        {
        }
    }
}
