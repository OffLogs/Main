using System.ComponentModel.DataAnnotations;

namespace OffLogs.Business.Common.Models.Api.Request.User
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