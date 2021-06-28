using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.WorkerService.LogProcessing
{
    public interface ILogProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}