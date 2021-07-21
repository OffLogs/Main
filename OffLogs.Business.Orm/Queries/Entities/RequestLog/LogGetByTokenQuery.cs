using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries.Entities.RequestLog
{
    public class RequestLogGetByTokenQuery : LinqAsyncQueryBase<RequestLogEntity, RequestLogGetByTokenCriteria, RequestLogEntity>
    {
        public RequestLogGetByTokenQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<RequestLogEntity> AskAsync(RequestLogGetByTokenCriteria criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.Query<RequestLogEntity>()
                .Where(q => q.Token == criterion.Token)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}