namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtApplicationService
    {
        public string GetToken();
        public string BuildJwt(int userId);
        public int? GetApplicationId(string jwtString = null);
        public bool IsValidJwt(string jwtString = null);
    }
}