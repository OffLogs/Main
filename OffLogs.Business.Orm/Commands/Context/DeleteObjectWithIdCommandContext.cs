using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;

namespace OffLogs.Business.Orm.Commands.Context
{
    public class DeleteObjectWithIdCommandContext<THasId> : ICommandContext
        where THasId : class, IHasId, new()
    {
        public THasId ObjectWithId { get; }

        public DeleteObjectWithIdCommandContext(THasId objectWithId)
        {
            ObjectWithId = objectWithId ?? throw new ArgumentNullException(nameof(objectWithId));
        }
    }

    public static class DeleteObjectWithIdCommandContextExtensions
    {
        public static Task DeleteAsync<THasId>(
            this IAsyncCommandBuilder commandBuilder,
            THasId objectWithId,
            CancellationToken cancellationToken = default
        ) where THasId : class, IHasId, new()
        {
            return commandBuilder.ExecuteAsync(
                new DeleteObjectWithIdCommandContext<THasId>(objectWithId),
                cancellationToken
            );
        }
    }
}
