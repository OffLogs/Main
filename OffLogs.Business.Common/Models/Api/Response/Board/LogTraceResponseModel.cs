using System;

namespace OffLogs.Business.Common.Models.Api.Response.Board
{
    public record LogTraceResponseModel
    {
        public long Id { get; set; }
        public string Trace { get; set; }
        public DateTime CreateTime { get; set; }

        public LogTraceResponseModel(LogTraceEntity entity)
        {
            Id = entity.Id;
            Trace = entity.Trace;
            CreateTime = entity.CreateTime;
        }
    }
}