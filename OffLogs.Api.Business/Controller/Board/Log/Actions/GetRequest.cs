using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class GetRequest: IRequest<LogDto>
    {
        [Required]
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }
    }
}
