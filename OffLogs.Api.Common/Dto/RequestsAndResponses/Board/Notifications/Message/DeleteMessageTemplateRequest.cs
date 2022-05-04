using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message
{
    public class DeleteMessageTemplateRequest: IRequest
    {
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }
    }
}
