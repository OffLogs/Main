using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Dto;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;
using ICriterion = Queries.Abstractions.ICriterion;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public record struct LogGetListCriteria(
        long ApplicationId,
        long Page,
        LogLevel? LogLevel,
        long? FavoriteForUserId
    ) : ICriterion;
    
    public class LogGetListQuery : LinqAsyncQueryBase<LogEntity, LogGetListCriteria, ListDto<LogEntity>>
    {
        private const string Query = @"
            select distinct log
                from LogEntity as log
                inner join fetch log.Application
                left join fetch log.FavoriteForUsers as favorite
                where log.Application.Id = :applicationId
                    and (:logLevel is null or log.Level = :logLevel)
                    and (:favoriteForUserId is null or favorite.Id = :favoriteForUserId)
                order by log.LogTime desc, log.CreateTime desc
        ";
        
        public LogGetListQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<ListDto<LogEntity>> AskAsync(
            LogGetListCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            var session = TransactionProvider.CurrentSession;
            var pageSize = GlobalConstants.ListPageSize * 2;
            var page = criterion.Page - 1;
            var offset = (int)((page <= 0 ? 0 : page) * pageSize);

            var logs = await session.CreateQuery(Query)
                .SetParameter("applicationId", criterion.ApplicationId)
                .SetParameter("logLevel", criterion.LogLevel)
                .SetParameter("favoriteForUserId", criterion.FavoriteForUserId)
                .SetFirstResult(offset)
                .SetMaxResults(pageSize)
                .ListAsync<LogEntity>(cancellationToken);
            var count = await session.Query<LogEntity>()
                .Where(record => record.Application.Id == criterion.ApplicationId)
                .LongCountAsync(cancellationToken);

            var favorites = await session.QueryOver<FavoriteLogEntity>()
                .Where(
                    Restrictions.In("Log", logs.ToArray())
                )
                .ListAsync(cancellationToken);
            foreach (var log in logs)
            {
                log.IsFavorite = favorites.Any(fl => fl.Log == log);
            }

            return new ListDto<LogEntity>(logs, count);
        }
    }
}
