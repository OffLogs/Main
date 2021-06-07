using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OffLogs.Business.Db.Dao
{
    public class CommonDao: BaseDao, ICommonDao
    {
        public CommonDao(IConfiguration configuration, ILogger<CommonDao> logger) : base(
            configuration.GetConnectionString("DefaultConnection"), 
            logger
        )
        {
        }
        
        public bool IsConnectionSuccessful()
        {
            using (var session = Session)
            {
                return session.IsConnected;    
            }
        }
    }
}