using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Connection;
using OffLogs.Business.Orm.Criteria.Entites;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Orm.Queries.Entities
{
    public class LogIsExistsByTokenQuery<THasId> : LinqAsyncQueryBase<THasId, LogIsExistsByTokenCriteria, bool>
        where THasId : class, IHasId, new()
    {
        public LogIsExistsByTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<bool> AskAsync(LogIsExistsByTokenCriteria criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.Query<LogEntity>()
                .Where(q => q.Token == criterion.Token)
                .AnyAsync(cancellationToken);
        }
    }
}