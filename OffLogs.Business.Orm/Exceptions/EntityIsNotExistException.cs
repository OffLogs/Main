using System;

namespace OffLogs.Business.Orm.Exceptions
{
    public class EntityIsNotExistException: Exception
    {
        public EntityIsNotExistException(string name = "") : base($"Record does not exist. {name}")
        {
        }
    }
}