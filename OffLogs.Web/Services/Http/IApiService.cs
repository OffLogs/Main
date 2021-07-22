using System.Threading.Tasks;
using Offlogs.Business.Api.Controller.Board.Application.Actions;
using Offlogs.Business.Api.Controller.Public.User.Actions;
using Offlogs.Business.Api.Controller.Public.User.Dto;
using Offlogs.Business.Api.Dto;
using Offlogs.Business.Api.Dto.Entities;

namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequest model);
        Task<bool> CheckIsLoggedInAsync(string token);
        Task<PaginatedListDto<ApplicationListItemDto>> GetApplications(GetListRequest request = null);
        Task<ApplicationDto> GetApplication(long logId);
        Task<PaginatedListDto<LogDto>> GetLogs(Offlogs.Business.Api.Controller.Board.Log.Actions.GetListRequest request);
        Task<LogDto> GetLog(long logId);
        Task<bool> LogSetIsFavorite(long logId, bool isFavorite);
    }
}