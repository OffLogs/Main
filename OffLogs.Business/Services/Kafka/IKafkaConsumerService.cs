using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Services.Kafka
{
    public interface IKafkaConsumerService: IDomainService
    {
        Task<long> ProcessLogsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null);
        Task<long> ProcessLogsAsync(CancellationToken cancellationToken);
    }
}