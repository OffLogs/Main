using System;
using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Connection;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries
{
    public abstract class LinqAsyncQueryBase<TCriterion, TResult> : IAsyncQuery<TCriterion, TResult>
        where TCriterion : ICriterion
    {
        protected readonly IDbSessionProvider TransactionProvider;

        protected LinqAsyncQueryBase(IDbSessionProvider transactionProvider)
        {
            this.TransactionProvider = transactionProvider ?? throw new ArgumentNullException(nameof(transactionProvider));
        }

        public abstract Task<TResult> AskAsync(TCriterion criterion, CancellationToken cancellationToken = default);
    }
}