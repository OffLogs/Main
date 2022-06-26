using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Common
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
