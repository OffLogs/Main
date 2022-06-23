using System.Threading.Tasks;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Entities.UserEmail
{
    public interface IUserEmailService: IDomainService
    {
        Task<UserEmailEntity> VerifyByToken(string token);

        Task<UserEmailEntity> Add(UserEntity user, string email);
    }
}
