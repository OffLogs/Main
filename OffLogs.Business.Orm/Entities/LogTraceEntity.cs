using System;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "log_traces")]
    public class LogTraceEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        [ManyToOne(
            ClassType = typeof(LogEntity), 
            Column = "log_id", 
            Lazy = Laziness.False,
            Cascade = "save-update"
        )]
        public virtual LogEntity Log { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "trace", Length = 2048, NotNull = true)]
        public virtual string Trace { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        public LogTraceEntity() {}

        public LogTraceEntity(string trace)
        {
            Trace = trace;
            CreateTime = DateTime.UtcNow;
        }
    }
}