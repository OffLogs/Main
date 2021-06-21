using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Communication
{
    public interface IKafkaProducerService
    {
        Task ProduceLogMessageAsync(LogEntity logEntity);
    }
}