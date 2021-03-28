using System;
using ServiceStack.DataAnnotations;

namespace OffLogs.Business.Db.Entity
{
    [Alias("log_properties")]
    public class LogPropertyEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        
        [Alias("log_id")]
        [References(typeof(LogEntity))]
        public long LogId { get; set; }
        
        [Alias("key")]
        public string Key { get; set; }
        
        [Alias("value")]
        public string Value { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }

        public LogPropertyEntity() {}

        public LogPropertyEntity(string key, string value)
        {
            Key = key;
            Value = value;
            CreateTime = DateTime.Now;
        }
    }
}