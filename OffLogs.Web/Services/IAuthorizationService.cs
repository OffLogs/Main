using System.Threading.Tasks;
using OffLogs.Business.Common.Models.Api.Request.User;

namespace OffLogs.Web.Services
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn();
        Task<bool> LoginAsync(LoginRequestModel model);
        Task LogoutAsync();
        Task<bool> IsHasJwtAsync();
        Task<string> GetJwtAsync();
        Task<bool> CheckIsLoggedInAsync();
    }
}