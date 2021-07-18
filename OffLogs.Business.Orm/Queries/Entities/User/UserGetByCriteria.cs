using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserGetByCriteria : ICriterion
    {
        public string UserName { get; }

        public UserGetByCriteria(string userName)
        {
            UserName = userName;
        }
    }
}