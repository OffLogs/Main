using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OffLogs.Business.Constants;
using OffLogs.Business.Mvc.Attribute.Constant;

namespace OffLogs.Api.Models.Request.Log.Serilog
{
    public class SerilogLogRequestModel
    {
        public DateTime Timestamp { get; set; }
        
        [StringLength(100)]
        [IsValidConstant(typeof(SerilogLogLevel))]
        public string Level { get; set; }
        
        [StringLength(1024)]
        public string MessageTemplate { get; set; }
        
        [StringLength(1024)]
        public string RenderedMessage { get; set; }
        
        [StringLength(5028)]
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