using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogIsExistsByTokenQuery : LinqAsyncQueryBase<LogEntity, GetByTokenCriteria, bool>
    {
        public LogIsExistsByTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<bool> AskAsync(
            GetByTokenCriteria criterion,
            CancellationToken cancellationToken = default
        )
        {
            return await TransactionProvider.CurrentSession.Query<LogEntity>()
                .Where(q => q.Token == criterion.Token)
                .AnyAsync(cancellationToken);
        }
    }
}
