using System.Threading.Tasks;
using OffLogs.Business.Entities;

namespace OffLogs.Business.Dao
{
    public interface IUserDao: ICommonDao
    {
        Task DeleteByUserName(string userName);
        Task<UserEntity> GetByUserName(string userName);
        Task<UserEntity> CreateNewUser(string userName, string email);
    }
}