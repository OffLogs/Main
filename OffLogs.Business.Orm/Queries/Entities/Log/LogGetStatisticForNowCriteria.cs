using OffLogs.Business.Common.Constants;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetStatisticForNowCriteria : ICriterion
    {
        public LogGetStatisticForNowCriteria(long? applicationId = null)
        {
            ApplicationId = applicationId;
        }

        public long? ApplicationId { get; }
    }
}