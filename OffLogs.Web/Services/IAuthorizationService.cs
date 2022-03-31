using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Web.Services
{
    public interface IAuthorizationService
    {
        Task<bool> LoginAsync(LoginRequest model);
        void Login(string jwtToken, string pem, string privateKeyBase64);
        Task LogoutAsync();
        Task<bool> CheckIsLoggedInAsync();
        string GetJwt();
    }
}
