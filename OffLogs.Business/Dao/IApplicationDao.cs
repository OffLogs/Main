using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Dao
{
    public interface IApplicationDao: ICommonDao
    {
        Task<ApplicationEntity> GetAsync(long applicationId);
        Task<ApplicationEntity> CreateNewApplication(long userId, string name);
        Task<ApplicationEntity> CreateNewApplication(UserEntity user, string name);
        Task<ApplicationEntity> UpdateApplication(long applicationId, string name);
        Task<bool> IsOwner(long userId, long applicationId);
        Task<bool> IsOwner(long userId, ApplicationEntity application);
        Task<(ICollection<ApplicationEntity>, long)> GetList(long userId, int page, int pageSize = 30);
    }
}