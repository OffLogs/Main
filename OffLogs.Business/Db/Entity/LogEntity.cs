using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Entity
{
    [Table("logs")]
    public class LogEntity
    {
        [Key]
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime CreateTime { get; set; }
        
        [Write(false)]
        [Computed]
        public ApplicationEntity Application { get; set; }
        
        [Write(false)]
        [Computed]
        public List<LogTraceEntity> Traces { get; set; } = new();

        [Write(false)] [Computed] 
        public List<LogPropertyEntity> Properties { get; set; } = new();
    }
}