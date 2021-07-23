using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto.Entities;

namespace OffLogs.Api.Business.Controller.Board.Application.Actions
{
    public class GetRequest : IRequest<ApplicationDto>
    {
        [Required]
        public long Id { get; set; }
    }
}
