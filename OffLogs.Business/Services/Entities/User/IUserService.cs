using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.User
{
    public interface IUserService: IDomainService
    {
        Task<UserEntity> CreateNewUser(string userName, string email);
    }
}