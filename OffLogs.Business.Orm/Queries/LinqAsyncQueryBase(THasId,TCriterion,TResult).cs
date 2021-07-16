using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Connection;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries
{
    public abstract class LinqAsyncQueryBase<THasId, TCriterion, TResult> : IAsyncQuery<TCriterion, TResult>
        where THasId : class, IHasId, new()
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