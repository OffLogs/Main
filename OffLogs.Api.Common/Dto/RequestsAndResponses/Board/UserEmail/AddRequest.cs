using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail
{
    public class AddRequest : IRequest<UserEmailDto>
    {
        [Required]
        [EmailAddress]
        [StringLength(255, MinimumLength = 3)]
        public string Email { get; set; }
    }
}
