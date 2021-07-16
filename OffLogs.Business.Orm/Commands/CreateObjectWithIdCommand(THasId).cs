using System;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Connection;

namespace OffLogs.Business.Orm.Commands
{
    public class CreateObjectWithIdCommand<THasId> : IAsyncCommand<CreateObjectWithIdCommandContext<THasId>>
        where THasId : class, IHasId, new()
    {
        private readonly IDbSessionProvider _sessionProvider;


        public CreateObjectWithIdCommand(IDbSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }


        public async Task ExecuteAsync(
            CreateObjectWithIdCommandContext<THasId> commandContext,
            CancellationToken cancellationToken = default)
        {
            await _sessionProvider.CurrentSession.SaveOrUpdateAsync(
                commandContext.ObjectWithId, 
                cancellationToken
            );
        }
    }
}