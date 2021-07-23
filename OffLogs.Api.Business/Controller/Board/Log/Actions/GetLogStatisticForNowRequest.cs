using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequest : IRequest<LogStatisticForNowDto>
    {
        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }
    }
}
