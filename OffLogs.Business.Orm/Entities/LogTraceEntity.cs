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
        [Column(Name = "encrypted_trace", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedTrace { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTimeOffset CreateTime { get; set; }
        
        public LogTraceEntity() {}

        public LogTraceEntity(byte[] encryptedTrace)
        {
            EncryptedTrace = encryptedTrace;
            CreateTime = DateTimeOffset.UtcNow;
        }
    }
}
