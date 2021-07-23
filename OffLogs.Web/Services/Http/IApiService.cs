using System.Threading.Tasks;
using OffLogs.Api.Business.Controller.Board.Application.Actions;
using OffLogs.Api.Business.Controller.Public.User.Actions;
using OffLogs.Api.Business.Controller.Public.User.Dto;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;

namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequest model);
        Task<bool> CheckIsLoggedInAsync(string token);
        Task<PaginatedListDto<ApplicationListItemDto>> GetApplications(GetListRequest request = null);
        Task<ApplicationDto> GetApplication(long logId);
        Task<PaginatedListDto<LogListItemDto>> GetLogs(OffLogs.Api.Business.Controller.Board.Log.Actions.GetListRequest request);
        Task<LogDto> GetLog(long logId);
        Task<bool> LogSetIsFavorite(long logId, bool isFavorite);
    }
}