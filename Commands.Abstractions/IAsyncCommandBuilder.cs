using System.Threading;
using System.Threading.Tasks;

namespace Commands.Abstractions
{
    public interface IAsyncCommandBuilder
    {
        Task ExecuteAsync<TCommandContext>(
            TCommandContext commandContext,
            CancellationToken cancellationToken = default) where TCommandContext : ICommandContext;
    }
}