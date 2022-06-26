using System;
using Domain.Abstractions;
using OffLogs.Business.Common.Resources;

namespace OffLogs.Business.Common.Exceptions.Common
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
