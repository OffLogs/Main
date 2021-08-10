using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using NHibernate.Linq;
using OffLogs.Business.Orm.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Business.Orm.Commands.Entities.Application
{
    public class ApplicationDeleteCommand : IAsyncCommand<ApplicationDeleteCommandContext>
    {
        private readonly IDbSessionProvider _sessionProvider;

        public ApplicationDeleteCommand(IDbSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public async Task ExecuteAsync(
            ApplicationDeleteCommandContext commandContext,
            CancellationToken cancellationToken = default
        )
        {
            await _sessionProvider.CurrentSession.Query<ApplicationEntity>()
                .Where(a => a.Id == commandContext.ApplicationId)
                .DeleteAsync();
        }
    }
}