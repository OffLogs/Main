using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Log.Actions
{
    public class GetRequest: IRequest<LogDto>
    {
        [Required]
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }
    }
}
