
using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Criteria.Entites
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