using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Exceptions;
using System.Threading.Tasks;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;

namespace OffLogs.Web.Services.Http
{
    public partial class ApiService
    {
        public async Task<MessageTemplateDto> NotificationMessageSet(SetMessageTemplateRequest templateRequest)
        {
            var response = await PostAuthorizedAsync<MessageTemplateDto>(
                MainApiUrl.NotificationMessageTemplateSet,
                templateRequest
            );
            if (response == null)
            {
                throw new ServerErrorException();
            }
            return response;
        }
    }
}
