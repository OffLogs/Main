using System.Data.SqlClient;

namespace OffLogs.Business.Db.Dao
{
    public interface ICommonDao
    {
        SqlConnection GetConnection();
    }
}