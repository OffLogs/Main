using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class LoginRequest : IRequest<LoginResponseDto>
    {
        [Required]
        public string SignedData { get; set; }
        
        [Required]
        [IsBase64]
        public string SignBase64 { get; set; }

        [Required]
        [IsBase64]
        public string PublicKeyBase64 { get; set; }
    }
}
