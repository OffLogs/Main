using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OffLogs.Business.Common.Resources;

namespace OffLogs.Business.Exceptions
{
    public class PermissionException : Exception, IDomainException
    {
        public PermissionException(): this(RG.Error_UserHasNotPermissions)
        {
        }

        public PermissionException(string message) : base(message)
        {
        }
    }
}
