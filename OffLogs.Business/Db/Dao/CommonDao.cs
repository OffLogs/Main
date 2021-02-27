using System.Data.SqlClient;

namespace OffLogs.Business.Db.Dao
{
    public class CommonDao: BaseDao, ICommonDao
    {
        public SqlConnection GetConnection()
        {
            return GetConnection();
        }
    }
}