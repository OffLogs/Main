using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.Log
{
    public class LogGetByTokenQuery : LinqAsyncQueryBase<LogEntity, GetByTokenCriteria, LogEntity>
    {
        public LogGetByTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<LogEntity> AskAsync(GetByTokenCriteria criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.Query<LogEntity>()
                .Where(q => q.Token == criterion.Token)
                .Fetch(e => e.Application)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}