using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<ListDto<NotificationRuleDto>> NotificationRuleGetList()
        {
            var response = await PostAuthorizedAsync<ListDto<NotificationRuleDto>>(
                MainApiUrl.NotificationRulesList
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
        
        public async Task<NotificationRuleDto> NotificationRuleSet(SetRuleRequest request)
        {
            var response = await PostAuthorizedAsync<NotificationRuleDto>(
                MainApiUrl.NotificationRulesSet,
                request
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
        
        public async Task NotificationRuleDelete(long id)
        {
            await PostAuthorizedAsync<object>(
                MainApiUrl.NotificationRuleDelete, 
                new IdRequest(id)    
            );
        }
    }
}
