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
        [References(typeof(ApplicationEntity))]
        public long ApplicationId { get; set; }
        
        [Alias("level")]
        public LogLevel Level { get; set; }
        
        [Alias("message")]
        public string Message { get; set; }
        
        [Alias("log_time")]
        public DateTime LogTime { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }
        
        [Reference]
        public ApplicationEntity Application { get; set; }
        
        [Reference]
        public List<LogTraceEntity> Traces { get; set; } = new();

        [Reference] 
        public List<LogPropertyEntity> Properties { get; set; } = new();
    }
}