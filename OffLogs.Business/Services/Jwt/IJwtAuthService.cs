namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtAuthService
    {
        public string GetToken();
        public string BuildJwt(int userId);
        public int? GetUserId(string jwtString = null);
    }
}