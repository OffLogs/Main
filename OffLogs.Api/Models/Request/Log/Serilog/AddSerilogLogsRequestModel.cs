using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Models.Request.Log.Serilog
{
    public class AddSerilogLogsRequestModel
    {
        [JsonPropertyName("events")]
        [ArrayLength(100)]
        public List<SerilogLogRequestModel> Events { get; set; } = new();
    }
}