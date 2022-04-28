using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class GetByTokenCriteria : ICriterion
    {
        public string Token { get; }

        public GetByTokenCriteria(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
