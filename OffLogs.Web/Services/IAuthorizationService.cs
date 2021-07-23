using System.Threading.Tasks;
using OffLogs.Api.Business.Controller.Public.User.Actions;

namespace OffLogs.Web.Services
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn();
        Task<bool> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        Task<bool> IsHasJwtAsync();
        Task<string> GetJwtAsync();
        Task<bool> CheckIsLoggedInAsync();
    }
}