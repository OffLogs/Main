﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Requests.Abstractions;
using OffLogs.Api.Frontend.Models.Request.Log.Common;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogsRequest: IRequest
    {
        [JsonPropertyName("logs")]
        [ArrayLength(100)]
        public List<CommonLogRequestModel> Logs { get; set; } = new();
    }
}