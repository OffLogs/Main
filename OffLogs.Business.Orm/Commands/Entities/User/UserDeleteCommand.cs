using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Commands.Entities.User
{
    public class DeleteUserCommand : IAsyncCommand<UserDeleteCommandContext>
    {
        private readonly IDbSessionProvider _sessionProvider;
        
        public DeleteUserCommand(IDbSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task ExecuteAsync(
            UserDeleteCommandContext commandContext,
            CancellationToken cancellationToken = default
        )
        {
            await _sessionProvider.CurrentSession.Query<UserEntity>()
                .Where(u => u.UserName == commandContext.UserName.ToLower())
                .DeleteAsync(cancellationToken);
        }
    }
}