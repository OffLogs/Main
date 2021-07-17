using Domain.Abstractions;

namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtAuthService: IDomainService
    {
        public string GetToken();
        public string BuildJwt(long userId);
        public long GetUserId(string jwtString = null);
        bool IsValidJwt(string jwtString = null);
    }
}