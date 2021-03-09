using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OffLogs.Api.Models.Request.Log.Common
{
    public class AddCommonLogsRequestModel
    {
        [Required]
        [JsonPropertyName("logs")]
        public List<CommonLogRequestModel> Logs { get; set; } = new();
    }
}