using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogIsFavoriteQuery : LinqAsyncQueryBase<LogIsFavoriteCriteria, bool>
    {
        public LogIsFavoriteQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<bool> AskAsync(LogIsFavoriteCriteria criterion, CancellationToken cancellationToken = default)
        {
            var result = await TransactionProvider.CurrentSession
                .GetNamedQuery("Log.getFavoriteCount")
                .SetParameter("userId", criterion.UserId)
                .SetParameter("logId", criterion.LogId)
                .UniqueResultAsync<long>(cancellationToken);
            return result > 0;
        }
    }
}