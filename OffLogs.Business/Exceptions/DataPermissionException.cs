using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Exceptions
{
    public class DataPermissionException : Exception, IDomainException
    {
        public DataPermissionException(): this("")
        {
        }

        public DataPermissionException(string message) : base($"Not enough rights. {message}")
        {
        }
    }
}
