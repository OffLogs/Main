using System;
using System.Net.Sockets;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.User
{
    public class UserGetByCriteria : ICriterion
    {
        public string UserName { get; }
        public string Email { get; }

        public UserGetByCriteria(
            string userName = null, 
            string email = null
        )
        {
            UserName = userName?.Trim().ToLower();
            Email = email?.Trim().ToLower();

            if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Email))
                throw new ArgumentNullException(nameof(UserName));
        }
    }
}