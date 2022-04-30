using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message
{
    public class GetMessageTemplateListRequest : IRequest<ListDto<MessageTemplateDto>>
    {
    }
}
