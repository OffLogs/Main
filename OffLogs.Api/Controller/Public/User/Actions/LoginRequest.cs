using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class LoginRequest: IRequest<LoginResponse>
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}