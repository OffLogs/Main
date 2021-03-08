using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface ILogDao
    {
        Task AddAsync(
            long applicationId,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        );

        Task<(IEnumerable<LogEntity>, int)> GetList(long applicationId, int page);
    }
}