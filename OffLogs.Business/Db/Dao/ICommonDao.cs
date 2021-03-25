using System.Data.SqlClient;
using Npgsql;

namespace OffLogs.Business.Db.Dao
{
    public interface ICommonDao
    {
        NpgsqlConnection GetConnection();
    }
}