using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail
{
    public class DeleteRequest : IRequest
    {
        [Required]
        public long Id { get; set; }
    }
}
