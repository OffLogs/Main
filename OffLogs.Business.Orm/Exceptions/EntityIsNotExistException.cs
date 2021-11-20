using System;
using Domain.Abstractions;

namespace OffLogs.Business.Orm.Exceptions
{
    public class EntityIsNotExistException: Exception, IDomainException
    {
        public EntityIsNotExistException(string name = "") : base($"Record does not exist. {name}")
        {
        }
    }
}
