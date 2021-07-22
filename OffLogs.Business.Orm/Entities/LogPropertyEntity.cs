using System;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "log_properties")]
    public class LogPropertyEntity: IEntity
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
        [Column(Name = "key", Length = 200, NotNull = true)]
        public virtual string Key { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "value", Length = 2048, NotNull = true)]
        public virtual string Value { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        public LogPropertyEntity() {}

        public LogPropertyEntity(string key, string value)
        {
            Key = key;
            Value = value;
            CreateTime = DateTime.UtcNow;
        }
        
        public LogPropertyEntity(string key, object value)
        {
            Key = key;
            try
            {
                Value = JsonConvert.SerializeObject(value);
            }
            catch (Exception)
            {
                Value = "";
            }
            CreateTime = DateTime.UtcNow;
        }
    }
}