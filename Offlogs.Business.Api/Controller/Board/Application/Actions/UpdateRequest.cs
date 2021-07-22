using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;

namespace Offlogs.Business.Api.Controller.Board.Application.Actions
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
