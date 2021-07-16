using System.Threading;
using System.Threading.Tasks;

namespace Commands.Abstractions
{
    public interface IAsyncCommand<in TCommandContext> where TCommandContext : ICommandContext
    {
        Task ExecuteAsync(TCommandContext commandContext, CancellationToken cancellationToken = default);
    }
}