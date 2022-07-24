using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public record struct LogIsFavoriteCriteria(long UserId, long LogId) : ICriterion;
    
    public class LogIsFavoriteQuery : LinqAsyncQueryBase<LogIsFavoriteCriteria, bool>
    {
        public LogIsFavoriteQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<bool> AskAsync(LogIsFavoriteCriteria criterion, CancellationToken cancellationToken = default)
        {
            var isFound = await TransactionProvider.CurrentSession
                .Query<UserEntity>()
                .Where(user => user.Id == criterion.UserId)
                .FetchMany(user => user.FavoriteLogs)
                .AnyAsync(user => user.FavoriteLogs.Any(log => log.Id == criterion.LogId), cancellationToken: cancellationToken);
            return isFound;
        }
    }
}
