using System;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogIsFavoriteCriteria : ICriterion
    {
        public LogIsFavoriteCriteria(long userId, long logId)
        {
            UserId = userId;
            LogId = logId;
        }

        public long UserId { get; }
        public long LogId { get; }
    }
}