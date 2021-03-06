namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtAuthService
    {
        public string GetToken();
        public string BuildJwt(long userId);
        public long? GetUserId(string jwtString = null);
    }
}