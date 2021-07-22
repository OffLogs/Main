using System;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Models.Api.Response.Board;

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
        
        [JsonIgnore]
        public virtual LogPropertyResponseModel ResponseModel
        {
            get
            {
                var model = new LogPropertyResponseModel()
                {
                    Id = Id,
                    Key = Key,
                    Value = Value,
                    CreateTime = CreateTime,
                };
                return model;
            }
        }
        
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