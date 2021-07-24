using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log
{
    public class LogShareRequest : IRequest<LogShareDto>
    {
        [Required]
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }
    }
}
