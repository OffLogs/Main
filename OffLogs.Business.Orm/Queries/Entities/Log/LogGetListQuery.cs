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

            var query = session.QueryOver<LogEntity>()
                .Where(record => record.Application.Id == criterion.ApplicationId);
            if (criterion.LogLevel.HasValue)
            {
                query.And(item => item.Level == criterion.LogLevel);
            }
            if (criterion.FavoriteForUserId.HasValue)
            {
                query.JoinQueryOver<UserEntity>(item => item.FavoriteForUsers)
                    .Where(user => user.Id == criterion.FavoriteForUserId);
            }

            var logs = await query.Clone()
                .Take(pageSize)
                .Skip(offset)
                .ListAsync<LogEntity>(cancellationToken);
            
            var count = await query.Clone()
                .RowCountInt64Async(cancellationToken);

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
