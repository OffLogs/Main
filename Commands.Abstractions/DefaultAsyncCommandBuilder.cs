using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commands.Abstractions
{
    public class DefaultAsyncCommandBuilder : IAsyncCommandBuilder
    {
        private readonly IAsyncCommandFactory _asyncCommandFactory;

        public DefaultAsyncCommandBuilder(IAsyncCommandFactory asyncCommandFactory)
        {
            _asyncCommandFactory = asyncCommandFactory ?? throw new ArgumentNullException(nameof(asyncCommandFactory));
        }

        public Task ExecuteAsync<TCommandContext>(
            TCommandContext commandContext,
            CancellationToken cancellationToken = default)
            where TCommandContext : ICommandContext
        {
            return _asyncCommandFactory.Create<TCommandContext>().ExecuteAsync(commandContext, cancellationToken);
        }
    }
}