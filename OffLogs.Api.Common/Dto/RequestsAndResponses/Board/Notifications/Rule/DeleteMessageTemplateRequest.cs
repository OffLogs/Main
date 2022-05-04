using Api.Requests.Abstractions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule
{
    public class DeleteRuleRequest: IRequest
    {
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }
    }
}
