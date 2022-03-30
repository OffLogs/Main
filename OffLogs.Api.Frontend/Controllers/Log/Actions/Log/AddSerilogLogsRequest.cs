using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Constant;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddSerilogLogsRequest: IRequest
    {
        [JsonPropertyName("events")]
        [ArrayLength(100)]
        public List<SerilogLogModel> Events { get; set; } = new();
    }
    
    public class SerilogLogModel
    {
        public DateTime Timestamp { get; set; }
        
        [Required]
        [StringLength(100)]
        [IsValidConstant(typeof(SerilogLogLevel))]
        public string Level { get; set; }
        
        [Required]
        [StringLength(1024)]
        public string MessageTemplate { get; set; }
        
        [Required]
        [StringLength(1024)]
        public string RenderedMessage { get; set; }
        
        [StringLength(30000)]
        public string Exception { get; set; }

        public Dictionary<string, object> Properties { get; set; } = new();
        
        [JsonIgnore]
        public SerilogLogLevel LogLevel
        {
            get => new SerilogLogLevel().FromString(Level);
        }
        
        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            // Member2 = "This value went into the data file during serialization.";
        }
    }
}
