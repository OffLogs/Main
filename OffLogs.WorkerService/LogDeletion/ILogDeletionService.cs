using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.WorkerService.LogDeletion
{
    public interface ILogDeletionService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}