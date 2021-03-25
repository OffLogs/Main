using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

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
        
        public NpgsqlConnection GetConnection()
        {
            return base.GetConnection();
        }
    }
}