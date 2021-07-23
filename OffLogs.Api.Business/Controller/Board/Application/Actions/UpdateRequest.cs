using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto.Entities;

namespace OffLogs.Api.Business.Controller.Board.Application.Actions
{
    public class UpdateRequest : IRequest<ApplicationDto>
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
