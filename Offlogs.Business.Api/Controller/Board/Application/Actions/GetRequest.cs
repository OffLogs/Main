using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;

namespace Offlogs.Business.Api.Controller.Board.Application.Actions
{
    public class GetRequest : IRequest<ApplicationDto>
    {
        [Required]
        public long Id { get; set; }
    }
}
