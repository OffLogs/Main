using OffLogs.Business.Common.Constants;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetListCriteria : ICriterion
    {
        public long ApplicationId { get; set; }
        
        public long Page { get; set; }
        
        public LogLevel? LogLevel { get; set; }
    }
}