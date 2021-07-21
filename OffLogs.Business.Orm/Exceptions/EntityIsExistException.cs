using System;

namespace OffLogs.Business.Orm.Exceptions
{
    public class EntityIsExistException: Exception
    {
        public EntityIsExistException(string name = "") : base($"Record is exists. {name}")
        {
        }
    }
}