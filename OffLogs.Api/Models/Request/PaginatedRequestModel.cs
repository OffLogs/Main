using System.ComponentModel.DataAnnotations;

namespace OffLogs.Api.Models.Request
{
    public class LoginRequestModel
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}