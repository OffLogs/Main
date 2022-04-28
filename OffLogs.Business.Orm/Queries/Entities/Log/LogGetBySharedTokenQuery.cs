using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetBySharedTokenQuery : LinqAsyncQueryBase<GetByTokenCriteria, LogEntity>
    {
        public LogGetBySharedTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<LogEntity> AskAsync(
            GetByTokenCriteria criterion, 
            CancellationToken cancellationToken = default
        )
        {
            return await TransactionProvider.CurrentSession.Query<LogEntity>()
                .Where(q => q.LogShares.Any(sh => sh.Token == criterion.Token))
                .Fetch(e => e.Application)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
