using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Dao
{
    public interface ILogDao: ICommonDao
    {
        Task<LogEntity> GetLogAsync(long logId);
        Task<LogEntity> GetLogAsync(string token);
        Task<LogEntity> AddAsync(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        );
        Task<LogEntity> AddAsync(LogEntity log);
        Task<(IEnumerable<LogEntity>, long)> GetList(
            long applicationId, 
            int page,
            LogLevel? logLevel = null,
            int pageSize = 30
        );
        Task<bool> IsOwner(long userId, long logId);
        Task<bool> SetIsFavoriteAsync(long logId, bool isFavorite);
        Task<bool> IsLogExists(string token);
    }
}