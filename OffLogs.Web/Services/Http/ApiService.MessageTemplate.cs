using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<ListDto<MessageTemplateDto>> MessageTemplateGetList()
        {
            var response = await PostAuthorizedAsync<ListDto<MessageTemplateDto>>(
                MainApiUrl.NotificationMessageTemplateList,
                new GetMessageTemplateListRequest()
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
        
        public async Task<MessageTemplateDto> MessageTemplateSet(SetMessageTemplateRequest request)
        {
            var response = await PostAuthorizedAsync<MessageTemplateDto>(
                MainApiUrl.NotificationMessageTemplateSet,
                request
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
    }
}
