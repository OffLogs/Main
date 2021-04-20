using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface ILogDao: ICommonDao
    {
        Task<LogEntity> AddAsync(
            long applicationId,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        );

        Task<(IEnumerable<LogEntity>, long)> GetList(long applicationId, int page, int pageSize = 30);
        Task<bool> IsOwner(long userId, long logId);
    }
}