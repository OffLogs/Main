using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Requests.Board.Log;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants.Permissions;


namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        #region User
        Task<Api.Common.Dto.RequestsAndResponses.Public.User.LoginResponseDto> LoginAsync(Api.Common.Dto.RequestsAndResponses.Public.User.LoginRequest model);
        Task<bool> CheckIsLoggedInAsync(string token);
        Task<Api.Common.Dto.RequestsAndResponses.Board.User.UsersListDto> FindUsers(string search);
        #endregion

        #region Application
        Task DeleteApplicationAsync(long id);
        Task<ApplicationDto> AddApplicationAsync(string name);
        Task<ApplicationDto> UpdateApplicationAsync(long Id, string name);
        Task<PaginatedListDto<ApplicationListItemDto>> GetApplicationsAsync(Api.Common.Dto.RequestsAndResponses.Board.Application.GetListRequest request = null);
        Task<ApplicationDto> GetApplicationAsync(long logId);
        #endregion

        #region Log
        Task<LogDto> GetLogAsync(long logId);
        Task<bool> LogSetIsFavoriteAsync(long logId, bool isFavorite);
        Task<LogStatisticForNowDto> LogGetStatisticForNowAsync(long? applicationId = null);
        Task<PaginatedListDto<LogListItemDto>> GetLogsAsync(Api.Common.Dto.RequestsAndResponses.Board.Log.GetListRequest request);
        #endregion
        
        #region Permissions

        Task<bool> PermissionAddAccess(
            PermissionAccessType accessType,
            long recipientId,
            long itemId
        );

        Task<bool> PermissionRemoveAccess(
            PermissionAccessType accessType,
            long recipientId,
            long itemId
        );
        #endregion
    }
}