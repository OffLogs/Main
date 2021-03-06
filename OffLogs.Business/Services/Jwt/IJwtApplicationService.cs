namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtApplicationService
    {
        public string GetToken();
        public string BuildJwt(long applicationId);
        public long? GetApplicationId(string jwtString = null);
        public bool IsValidJwt(string jwtString = null);
    }
}