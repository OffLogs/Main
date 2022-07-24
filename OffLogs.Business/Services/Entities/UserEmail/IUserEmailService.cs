using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Services.Entities.UserEmail
{
    public interface IUserEmailService: IDomainService
    {
        Task<UserEmailEntity> VerifyByTokenAsync(string token);

        Task<UserEmailEntity> AddAsync(UserEntity user, string email);
    }
}
