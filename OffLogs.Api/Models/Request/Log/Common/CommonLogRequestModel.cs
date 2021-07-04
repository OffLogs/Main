using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Constant;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using OffLogs.Business.Constants;

namespace OffLogs.Api.Models.Request.Log.Common
{
    public class CommonLogRequestModel
    {
        public DateTime Timestamp { get; set; }
        
        [StringLength(100)]
        [IsValidConstant(typeof(SiteLogLevel))]
        public string Level { get; set; }
        
        [StringLength(1024)]
        public string Message { get; set; }
        
        [StringArrayLength(512)]
        public List<string> Traces { get; set; } = new();

        public Dictionary<string, string> Properties { get; set; } = new();
        
        [JsonIgnore]
        public LogLevel LogLevel
        {
            get
            {
                var level = Level.ToUpper();
                if (level == "E")
                {
                    return LogLevel.Error;
                }
                if (level == "W")
                {
                    return LogLevel.Warning;
                }
                if (level == "F")
                {
                    return LogLevel.Fatal;
                }
                if (level == "I")
                {
                    return LogLevel.Information;
                }
                if (level == "D")
                {
                    return LogLevel.Debug;
                }
                return default;
            }
        }
    }
}