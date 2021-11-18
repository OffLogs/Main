using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserGetPendingCriteria: ICriterion
    {
        public string Token { get; }

        public UserGetPendingCriteria(string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            Token = token;
        }
    }
}