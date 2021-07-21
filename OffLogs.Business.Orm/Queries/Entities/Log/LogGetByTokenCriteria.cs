using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetByTokenCriteria : ICriterion
    {
        public string Token { get; }

        public LogGetByTokenCriteria(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}