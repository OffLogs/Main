using Domain.Abstractions;

namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtApplicationService: IDomainService
    {
        public string BuildJwt(long applicationId);
        public long? GetApplicationId(string jwtString);
        public bool IsValidJwt(string jwtString);
    }
}