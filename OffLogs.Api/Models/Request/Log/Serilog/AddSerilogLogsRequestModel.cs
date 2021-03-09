using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OffLogs.Business.Mvc.Attribute.Validation;

namespace OffLogs.Api.Models.Request.Log.Serilog
{
    public class AddSerilogLogsRequestModel
    {
        [ArrayLength(100)]
        [JsonPropertyName("events")]
        public List<SerilogLogRequestModel> Events { get; set; } = new();
    }
}