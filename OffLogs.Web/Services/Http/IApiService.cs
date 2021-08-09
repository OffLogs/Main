using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Requests.Board.Log;
using System.Threading.Tasks;


namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        #region User
        Task<Api.Common.Dto.RequestsAndResponses.Public.User.LoginResponseDto> LoginAsync(Api.Common.Dto.RequestsAndResponses.Public.User.LoginRequest model);
        Task<bool> CheckIsLoggedInAsync(string token);
        #endregion

        #region Application
        Task<ApplicationDto> AddApplication(string name);
        Task<ApplicationDto> UpdateApplication(long Id, string name);
        Task<PaginatedListDto<ApplicationListItemDto>> GetApplications(Api.Common.Dto.RequestsAndResponses.Board.Application.GetListRequest request = null);
        Task<ApplicationDto> GetApplication(long logId);
        #endregion

        #region Log
        Task<LogDto> GetLog(long logId);
        Task<bool> LogSetIsFavorite(long logId, bool isFavorite);
        Task<LogStatisticForNowDto> LogGetStatisticForNow(long? applicationId = null);
        Task<PaginatedListDto<LogListItemDto>> GetLogs(Api.Common.Dto.RequestsAndResponses.Board.Log.GetListRequest request);
        #endregion
    }
}