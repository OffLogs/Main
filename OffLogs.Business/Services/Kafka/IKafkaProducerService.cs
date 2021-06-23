using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Kafka
{
    public interface IKafkaProducerService
    {
        Task ProduceLogMessageAsync(string applicationJwt, LogEntity logEntity);
    }
}