using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Queries
{
    public class FindByIdCriteria : ICriterion
    {
        public readonly long Id;

        public FindByIdCriteria(long Id)
        {
            this.Id = Id;
        }
    }

    public static class FindByIdCriterionExtensions
    {
        public static Task<THasId> FindByIdAsync<THasId>(
            this IAsyncQueryBuilder asyncQueryBuilder,
            long id,
            CancellationToken cancellationToken = default)
            where THasId : class, IHasId, new()
        {
            return asyncQueryBuilder
                .For<THasId>()
                .WithAsync(new FindByIdCriteria(id), cancellationToken);
        }
    }
}