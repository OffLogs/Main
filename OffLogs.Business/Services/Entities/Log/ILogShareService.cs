using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Entities.Log
{
    public interface ILogShareService: IDomainService
    {
        Task<LogShareEntity> Share(LogEntity log);

        Task DeleteShare(LogEntity log);
    }
}
