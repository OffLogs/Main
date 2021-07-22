using System.Threading.Tasks;
using Offlogs.Business.Api.Controller.Public.User.Actions;

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