using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Criterias;

public class GetByTokenCriteria : ICriterion
{
    public string Token { get; }
        
    public GetByTokenCriteria(string token)
    {
        Token = token;
    }
}
