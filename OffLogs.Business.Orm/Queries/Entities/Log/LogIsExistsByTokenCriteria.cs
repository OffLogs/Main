using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogIsExistsByTokenCriteria : ICriterion
    {
        public string Token { get; }

        public LogIsExistsByTokenCriteria(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}