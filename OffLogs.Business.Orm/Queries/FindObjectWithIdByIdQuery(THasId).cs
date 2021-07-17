using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Connection;
using OffLogs.Business.Orm.Criteria;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Queries
{
    public class FindObjectWithIdByIdQuery<THasId> : LinqAsyncQueryBase<THasId, FindById, THasId>
        where THasId : class, IHasId, new()
    {
        public FindObjectWithIdByIdQuery(IDbSessionProvider transactionProvider) 
            : base(transactionProvider)
        {
        }

        public override async Task<THasId> AskAsync(FindById criterion, CancellationToken cancellationToken = default)
        {
            return await TransactionProvider.CurrentSession.GetAsync<THasId>(
                criterion.Id, 
                cancellationToken
            );
        }
    }
}