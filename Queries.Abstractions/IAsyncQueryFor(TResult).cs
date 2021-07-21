using System.Threading;
using System.Threading.Tasks;

namespace Queries.Abstractions
{
    public interface IAsyncQueryFor<TResult>
    {
        Task<TResult> WithAsync<TCriterion>(
            TCriterion criterion,
            CancellationToken cancellationToken = default)
            where TCriterion : ICriterion;
    }
}