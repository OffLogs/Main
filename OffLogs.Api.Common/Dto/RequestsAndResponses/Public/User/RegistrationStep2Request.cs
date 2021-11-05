using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class RegistrationStep2Request : IRequest<RegistrationStep2ResponseDto>
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string Password { get; set; }
    }
}