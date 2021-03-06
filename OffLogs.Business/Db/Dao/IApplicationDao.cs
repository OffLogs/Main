using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface IApplicationDao: ICommonDao
    {
        Task<ApplicationEntity> CreateNewApplication(int userId, string name);
    }
}