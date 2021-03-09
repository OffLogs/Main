using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OffLogs.Api.Models.Request.Log.Serilog;

namespace OffLogs.Api.Models.Request.User
{
    public record LoginResponseModel
    {
        public string Token { get; set; }

        public LoginResponseModel() {}
    }
}