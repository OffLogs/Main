using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class RegistrationStep1Request : IRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // TODO: Added reCaptcha validator
        [Required]
        [StringLength(512)]
        public string ReCaptcha { get; set; }
    }
}