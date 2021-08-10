using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application
{
    public class DeleteRequest : IRequest
    {
        [Required]
        public long Id { get; set; }
    }
}
