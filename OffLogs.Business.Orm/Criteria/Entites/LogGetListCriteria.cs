
using System;
using OffLogs.Business.Common.Constants;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Criteria.Entites
{
    public class LogGetListCriteria : ICriterion
    {
        public long ApplicationId { get; set; }
        
        public long Page { get; set; }
        
        public LogLevel? LogLevel { get; set; }
        
        public int PageSize { get; set; }
    }
}