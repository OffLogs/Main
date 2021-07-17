using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.Application
{
    public interface IApplicationService: IDomainService
    {
        Task<ApplicationEntity> CreateNewApplication(UserEntity user, string name);
    }
}