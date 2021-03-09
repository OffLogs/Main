using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OffLogs.Api.Models.Request.Log.Serilog;

namespace OffLogs.Api.Models.Request.User
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