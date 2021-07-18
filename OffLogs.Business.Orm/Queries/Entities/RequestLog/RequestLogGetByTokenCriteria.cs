using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.RequestLog
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