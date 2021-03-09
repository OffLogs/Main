using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OffLogs.Api.Models.Request.Log.Common
{
    public class AddCommonLogsRequestModel
    {
        [JsonPropertyName("events")]
        public List<SerilogLogRequestModel> Events { get; set; } = new();
    }
}