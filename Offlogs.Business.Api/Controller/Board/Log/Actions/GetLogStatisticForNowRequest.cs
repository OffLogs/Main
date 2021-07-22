using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Log.Actions
{
    public class GetLogStatisticForNowRequest : IRequest<LogStatisticForNowDto>
    {
        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }
    }
}
