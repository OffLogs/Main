using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Commands.Entities.Log
{
    public class LogDeleteSpoiledCommand : IAsyncCommand<LogDeleteSpoiledCommandContext>
    {
        private readonly IDbSessionProvider _sessionProvider;

        public LogDeleteSpoiledCommand(IDbSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task ExecuteAsync(
            LogDeleteSpoiledCommandContext commandContext,
            CancellationToken cancellationToken = default
        )
        {
            var endTime = DateTime.UtcNow.AddDays(-30);
            await _sessionProvider.CurrentSession.GetNamedQuery("Log.deleteSpoiled")
                .SetParameter("endTime", endTime)
                .ExecuteUpdateAsync(cancellationToken);
        }
    }
}
