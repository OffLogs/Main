
using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Criteria.Entites
{
    public class RequestLogGetByTokenCriteria : ICriterion
    {
        public string Token { get; }

        public RequestLogGetByTokenCriteria(string token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}