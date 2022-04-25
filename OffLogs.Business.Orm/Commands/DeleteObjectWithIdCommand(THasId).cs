using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using OffLogs.Business.Orm.Commands.Context;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Commands
{
    public class DeleteObjectWithIdCommand<THasId> : IAsyncCommand<DeleteObjectWithIdCommandContext<THasId>>
        where THasId : class, IHasId, new()
    {
        private readonly IDbSessionProvider _sessionProvider;
        
        public DeleteObjectWithIdCommand(IDbSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task ExecuteAsync(
            DeleteObjectWithIdCommandContext<THasId> commandContext,
            CancellationToken cancellationToken = default
        )
        {
            await _sessionProvider.CurrentSession.DeleteAsync(
                commandContext.ObjectWithId, 
                cancellationToken
            );
        }
    }
}
