using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Db.Dao
{
    public interface IUserDao: ICommonDao
    {
        Task DeleteByUserName(string userName);
        Task<UserEntity> CreateNewUser(string userName, string email);
    }
}