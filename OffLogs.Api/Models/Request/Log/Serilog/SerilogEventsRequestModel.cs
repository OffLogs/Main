using System.Collections.Generic;
using Newtonsoft.Json;

namespace OffLogs.Api.Models.Request.Log.Serilog
{
    public class SerilogEventsRequestModel
    {
        public List<SerilogLogRequestModel> Events { get; set; }
    }
}