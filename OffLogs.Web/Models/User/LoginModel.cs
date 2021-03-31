using System;
using System.ComponentModel.DataAnnotations;

namespace OffLogs.Web.Models.User
{
    public class LoginModel
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Password { get; set; }
    }
}