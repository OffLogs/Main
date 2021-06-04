using System.Threading.Tasks;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Common.Models.Api.Response.Board;

namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        Task<LoginResponseModel> LoginAsync(LoginRequestModel model);
        Task<bool> CheckIsLoggedInAsync(string token);
        Task<PaginatedResponseModel<ApplicationResponseModel>> GetApplications(PaginatedRequestModel request);
        Task<PaginatedResponseModel<LogResponseModel>> GetLogs(LogListRequestModel request);
    }
}