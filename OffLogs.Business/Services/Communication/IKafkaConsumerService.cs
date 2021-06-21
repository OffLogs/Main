using System.Threading;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Communication
{
    public interface IKafkaConsumerService
    {
        Task<long> ProcessLogsAsync(CancellationToken cancellationToken = default);
    }
}