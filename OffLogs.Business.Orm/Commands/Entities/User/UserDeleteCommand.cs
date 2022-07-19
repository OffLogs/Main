using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
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
            var email = commandContext.Email ?? "";
            var userName = commandContext.UserName ?? "";
            await _sessionProvider.CurrentSession.Query<UserEntity>()
                .Where(
                    u => u.UserName == userName.Trim().ToLower()
                        || u.Email == email.Trim().ToLower()
                )
                .DeleteAsync(cancellationToken);
        }
    }
}
