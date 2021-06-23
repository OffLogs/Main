using System.Threading;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Kafka
{
    public interface IKafkaConsumerService
    {
        Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken cancellationToken = default);
    }
}