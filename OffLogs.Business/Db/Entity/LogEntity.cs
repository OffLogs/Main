using System;
using System.Collections.Generic;
using OffLogs.Business.Constants;
using ServiceStack.DataAnnotations;

namespace OffLogs.Business.Db.Entity
{
    [Alias("logs")]
    public class LogEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        
        [Alias("application_id")]
        [ForeignKey(typeof(ApplicationEntity))]
        public long ApplicationId { get; set; }
        
        [Alias("level")]
        public LogLevel Level { get; set; }
        
        [Alias("message")]
        public string Message { get; set; }
        
        [Alias("log_time")]
        public DateTime LogTime { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }
        
        [Ignore]
        public ApplicationEntity Application { get; set; }
        
        [Ignore]
        public List<LogTraceEntity> Traces { get; set; } = new();

        [Ignore] 
        public List<LogPropertyEntity> Properties { get; set; } = new();
    }
}