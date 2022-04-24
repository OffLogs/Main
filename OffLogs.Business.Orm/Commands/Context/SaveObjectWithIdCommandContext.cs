using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;

namespace OffLogs.Business.Orm.Commands.Context
{
    public class SaveObjectWithIdCommandContext<THasId> : ICommandContext
        where THasId : class, IHasId, new()
    {
        public SaveObjectWithIdCommandContext(THasId objectWithId)
        {
            ObjectWithId = objectWithId ?? throw new ArgumentNullException(nameof(objectWithId));
        }

        public THasId ObjectWithId { get; }
    }

    public static class CreateObjectWithIdCommandContextExtensions
    {
        public static Task SaveAsync<THasId>(
            this IAsyncCommandBuilder commandBuilder,
            THasId objectWithId,
            CancellationToken cancellationToken = default
        ) where THasId : class, IHasId, new()
        {
            return commandBuilder.ExecuteAsync(
                new SaveObjectWithIdCommandContext<THasId>(objectWithId),
                cancellationToken
            );
        }
    }
}
