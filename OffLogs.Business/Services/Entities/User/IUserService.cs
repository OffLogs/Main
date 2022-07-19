using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Entities.User
{
    public interface IUserService: IDomainService
    {
        Task<UserEntity> CreatePendingUser(string email, string userName = null);

        Task<(UserEntity, string)> ActivateUser(
            long userId,
            string privateKeyPassword
        );
    }
}