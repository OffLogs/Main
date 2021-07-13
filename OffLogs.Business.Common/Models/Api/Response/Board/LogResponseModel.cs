using System;
using System.Collections.Generic;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Business.Common.Models.Api.Response.Board
{
    public record LogResponseModel
    {
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }
        
        public List<string> Traces { get; set; } = new();
        public Dictionary<string, string> Properties { get; set; } = new();

        public LogResponseModel()
        {
        }
    }
}