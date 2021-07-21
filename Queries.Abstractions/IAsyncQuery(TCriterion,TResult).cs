using System.Threading;
using System.Threading.Tasks;

namespace Queries.Abstractions
{
    public interface IAsyncQuery<in TCriterion, TResult> where TCriterion : ICriterion
    {
        Task<TResult> AskAsync(TCriterion criterion, CancellationToken cancellationToken = default);
    }
}