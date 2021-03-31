using System;
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

        public LogTraceEntity() {}

        public LogTraceEntity(string trace)
        {
            Trace = trace;
            CreateTime = DateTime.Now;
        }
    }
}