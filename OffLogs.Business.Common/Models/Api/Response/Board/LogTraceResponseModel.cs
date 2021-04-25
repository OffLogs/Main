using System;

namespace OffLogs.Business.Common.Models.Api.Response.Board
{
    public record LogTraceResponseModel
    {
        public long Id { get; set; }
        public string Trace { get; set; }
        public DateTime CreateTime { get; set; }
    }
}