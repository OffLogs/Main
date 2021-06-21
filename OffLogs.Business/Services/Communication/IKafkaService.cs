using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Communication
{
    public interface IKafkaService
    {
        Task ProduceLogMessageAsync(LogEntity logEntity);
    }
}