using System.Data.SqlClient;
using Npgsql;
using SimpleStack.Orm;

namespace OffLogs.Business.Db.Dao
{
    public interface ICommonDao
    {
        OrmConnection GetConnection();
    }
}