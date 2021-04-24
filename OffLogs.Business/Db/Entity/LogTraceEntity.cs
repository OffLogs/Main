using System;
using OffLogs.Business.Common.Models.Api.Response.Board;
using ServiceStack.DataAnnotations;

namespace OffLogs.Business.Db.Entity
{
    [Alias("log_traces")]
    public class LogTraceEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        
        [Alias("log_id")]
        [References(typeof(LogEntity))]
        public long LogId { get; set; }
        
        [Alias("trace")]
        public string Trace { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }

        [Computed]
        public LogTraceResponseModel ResponseModel
        {
            get
            {
                var model = new LogTraceResponseModel()
                {
                    Id = Id,
                    Trace = Trace,
                    CreateTime = CreateTime,
                };
                return model;
            }
        }
        
        public LogTraceEntity() {}

        public LogTraceEntity(string trace)
        {
            Trace = trace;
            CreateTime = DateTime.Now;
        }
    }
}