using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class LoginRequest : IRequest<LoginResponseDto>
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}