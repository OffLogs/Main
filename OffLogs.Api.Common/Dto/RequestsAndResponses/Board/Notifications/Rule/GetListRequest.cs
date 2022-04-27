using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule
{
    public class GetListRequest : IRequest<ListDto<NotificationRuleDto>>
    {
    }
}
