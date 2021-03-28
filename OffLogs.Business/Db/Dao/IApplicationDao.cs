using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface IApplicationDao: ICommonDao
    {
        Task<ApplicationEntity> CreateNewApplication(long userId, string name);
        Task<bool> IsOwner(long userId, long applicationId);
    }
}