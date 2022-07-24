using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Entities.Application
{
    public interface IApplicationService: IDomainService
    {
        Task<ApplicationEntity> CreateNewApplication(UserEntity user, string name);
        Task<ApplicationEntity> UpdateApplication(long applicationId, string name);
        Task ShareForUser(ApplicationEntity application, UserEntity user);
        Task RemoveShareForUser(ApplicationEntity application, UserEntity user);
        Task Delete(long applicationId);
    }
}