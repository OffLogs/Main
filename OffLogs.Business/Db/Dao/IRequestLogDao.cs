using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface IRequestLogDao: ICommonDao
    {
        Task<RequestLogEntity> AddAsync(
            RequestLogType type,
            string clientIp,  
            string data
        );
        Task<RequestLogEntity> AddAsync(
            RequestLogType type,
            string clientIp,  
            object data
        );
    }
}