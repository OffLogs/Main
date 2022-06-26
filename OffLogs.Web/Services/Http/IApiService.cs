using System.Collections.Generic;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Web.Services.Http
{
    public interface IApiService
    {
        #region User
        Task<LoginResponseDto> LoginAsync(Api.Common.Dto.RequestsAndResponses.Public.User.LoginRequest model);
        Task<bool> CheckIsLoggedInAsync(string token);
        Task<UsersListDto> FindUsers(string search);
        #endregion

        #region UserEmails
        Task<UserEmailsListDto> UserEmailsGetList();
        Task<UserEmailDto> UserEmailAddAsync(string email);
        Task<ApplicationDto> UserEmailDeleteAsync(long id);
        #endregion
        
        #region Application
        Task DeleteApplicationAsync(long id);
        Task<ApplicationDto> AddApplicationAsync(string name);
        Task<ApplicationDto> UpdateApplicationAsync(long Id, string name);
        Task<PaginatedListDto<ApplicationListItemDto>> GetApplicationsAsync(Api.Common.Dto.RequestsAndResponses.Board.Application.GetListRequest request = null);
        Task<ApplicationDto> GetApplicationAsync(long logId);
        Task<UsersListDto> ApplicationGetSharedUsersAsync(long applicationId);
        #endregion

        #region Log
        Task<LogDto> GetLogAsync(long logId, string privateKeyBase64);
        Task LogSetIsFavoriteAsync(long logId, bool isFavorite);
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

        #region Registration

        Task<bool> RegistrationStep1Async(RegistrationStep1Request model);
        Task<RegistrationStep2ResponseDto> RegistrationStep2Async(RegistrationStep2Request model);

        #endregion

        #region Notification Rule

        Task<ListDto<NotificationRuleDto>> NotificationRuleGetList();
        Task<NotificationRuleDto> NotificationRuleSet(SetRuleRequest request);
        Task NotificationRuleDelete(long id);

        #endregion

        #region Message Template

        Task<ListDto<MessageTemplateDto>> MessageTemplateGetList();
        Task<MessageTemplateDto> MessageTemplateSet(SetMessageTemplateRequest request);
        Task MessageTemplateDelete(long id);

        #endregion
    }
}
