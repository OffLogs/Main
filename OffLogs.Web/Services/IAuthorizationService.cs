using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Web.Services
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn();
        Task<bool> LoginAsync(LoginRequest model);
        Task<bool> LoginAsync(string jwtToken);
        Task LogoutAsync();
        Task<bool> IsHasJwtAsync();
        Task<string> GetJwtAsync();
        Task<bool> CheckIsLoggedInAsync();
    }
}
