using OffLogs.Business.Common.Constants;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class GetByApplicationOrUserCriteria : ICriterion
    {
        public long UserId { get; }
        public long? ApplicationId { get; }

        public GetByApplicationOrUserCriteria(
            long userId,
            long? applicationId = null
        )
        {
            ApplicationId = applicationId;
            UserId = userId;
        }
    }
}
