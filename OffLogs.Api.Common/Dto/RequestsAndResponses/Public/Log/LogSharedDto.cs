using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log
{
    public class LogSharedDto: IResponse
    {
        public long Id { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }

        public List<string> Traces { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();
    }
}
