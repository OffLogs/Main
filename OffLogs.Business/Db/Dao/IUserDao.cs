using System.Data.SqlClient;
using System.Threading.Tasks;
using OffLogs.Business.Db.Entities;

namespace OffLogs.Business.Db.Dao
{
    public interface IUserDao: ICommonDao
    {
        Task DeleteByUserName(string userName);
        Task<UserEntity> GetByUserName(string userName);
        Task<UserEntity> CreateNewUser(string userName, string email);
    }
}