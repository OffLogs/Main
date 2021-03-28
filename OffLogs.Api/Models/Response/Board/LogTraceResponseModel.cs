using System;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Models.Response.Board
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