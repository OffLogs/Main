using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application
{
    public class AddRequest : IRequest<ApplicationDto>
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
