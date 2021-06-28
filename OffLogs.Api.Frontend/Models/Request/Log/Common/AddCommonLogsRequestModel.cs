using System.Collections.Generic;
using System.Text.Json.Serialization;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Frontend.Models.Request.Log.Common
{
    public class AddCommonLogsRequestModel
    {
        [JsonPropertyName("logs")]
        [ArrayLength(100)]
        public List<CommonLogRequestModel> Logs { get; set; } = new();
    }
}