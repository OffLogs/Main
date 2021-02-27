namespace OffLogs.Business.Services
{
    public interface IJwtService
    {
        public string GetToken();
        public string BuildJwt(int userId);
        public int? GetUserId(string jwtString = null);
    }
}