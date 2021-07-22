using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Controller.Public.User.Dto;

namespace Offlogs.Business.Api.Controller.Public.User.Actions
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