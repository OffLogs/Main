using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.Application
{
    public interface IApplicationService: IDomainService
    {
        Task<ApplicationEntity> CreateNewApplication(UserEntity user, string name);
        Task<bool> IsOwner(long userId, ApplicationEntity application);
        Task<bool> IsOwner(long userId, long applicationId);
        Task<ApplicationEntity> UpdateApplication(long applicationId, string name);
        Task ShareForUser(ApplicationEntity application, UserEntity user);
    }
}