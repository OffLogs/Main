using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Commands.Entities.Log
{
    public record struct LogSetIsFavoriteCommandContext(long UserId, long LogId, bool IsFavorite) : ICommandContext;
    
    public class SetIsFavoriteCommand : IAsyncCommand<LogSetIsFavoriteCommandContext>
    {
        private readonly IDbSessionProvider _sessionProvider;

        public SetIsFavoriteCommand(
            IDbSessionProvider sessionProvider
        )
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task ExecuteAsync(
            LogSetIsFavoriteCommandContext commandContext,
            CancellationToken cancellationToken = default
        )
        {
            var user = await _sessionProvider.CurrentSession
                .Query<UserEntity>()
                .FirstOrDefaultAsync(
                    user => user.Id == commandContext.UserId,
                    cancellationToken: cancellationToken
                );
            if (user != null)
            {
                var log = await _sessionProvider.CurrentSession
                    .Query<LogEntity>()
                    .FirstOrDefaultAsync(
                        log => log.Id == commandContext.LogId,
                        cancellationToken: cancellationToken
                    );
                if (commandContext.IsFavorite)
                {
                    user.FavoriteLogs.Add(log);
                }
                else
                {
                    user.FavoriteLogs.Remove(log);
                }

                await _sessionProvider.CurrentSession.SaveAsync(user, cancellationToken);
            }
        }
    }
}
