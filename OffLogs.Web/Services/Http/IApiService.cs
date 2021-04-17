using System.Threading.Tasks;
using OffLogs.Business.Common.Models.Api.Request.User;

namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<bool> CheckIsLoggedInAsync(string token);
    }
}