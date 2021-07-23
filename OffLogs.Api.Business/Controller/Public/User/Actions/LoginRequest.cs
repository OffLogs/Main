using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Controller.Public.User.Dto;

namespace OffLogs.Api.Business.Controller.Public.User.Actions
{
    public class LoginRequest: IRequest<LoginResponseDto>
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}