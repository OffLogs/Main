using Domain.Abstractions;

namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtApplicationService: IDomainService
    {
        public string GetToken();
        public string BuildJwt(long applicationId);
        public long? GetApplicationId(string jwtString = null);
        public bool IsValidJwt(string jwtString = null);
    }
}