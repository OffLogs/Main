using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Queries.Abstractions;

namespace OffLogs.Business.Orm.Criteria
{
    public class FindById : ICriterion
    {
        public readonly long Id;

        public FindById(long Id)
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
                .WithAsync(new FindById(id), cancellationToken);
        }
    }
}