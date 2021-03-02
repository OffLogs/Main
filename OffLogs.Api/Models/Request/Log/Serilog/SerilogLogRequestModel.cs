using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Dictionary<string, string> Properties { get; set; } = new();
    }
}