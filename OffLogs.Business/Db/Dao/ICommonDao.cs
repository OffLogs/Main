using System.Data;

namespace OffLogs.Business.Db.Dao
{
    public interface ICommonDao
    {
        IDbConnection GetConnection();
    }
}