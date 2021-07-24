using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetBySharedTokenCriteria : ICriterion
    {
        public string Token { get; }

        public LogGetBySharedTokenCriteria(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}