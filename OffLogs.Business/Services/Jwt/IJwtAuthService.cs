using Domain.Abstractions;

namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtAuthService: IDomainService
    {
        public string BuildJwt(long userId);
        public long GetUserId(string jwtString);
        bool IsValidJwt(string jwtString);
    }
}