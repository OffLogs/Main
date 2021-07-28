using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;

namespace OffLogs.Business.Services.Kafka.Consumer
{
    public interface IKafkaNotificationsConsumerService : IDomainService
    {
        Task<long> ProcessNotificationsAsync(bool isInfiniteLoop = true, CancellationToken? cancellationToken = null);
        Task<long> ProcessNotificationsAsync(CancellationToken cancellationToken);
    }
}