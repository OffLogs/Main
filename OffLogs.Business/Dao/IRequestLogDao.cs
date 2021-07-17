using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Dao
{
    public interface IRequestLogDao: ICommonDao
    {
        Task<RequestLogEntity> AddAsync(
            RequestLogType type,
            string clientIp,  
            string data,
            string token = null
        );
        Task<RequestLogEntity> AddAsync(
            RequestLogType type,
            string clientIp,  
            object data,
            string token = null
        );
        Task<RequestLogEntity> GetByTokenAsync(string token);
    }
}