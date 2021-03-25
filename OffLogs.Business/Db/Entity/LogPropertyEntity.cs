using System;
using Dapper.Contrib.Extensions;

namespace OffLogs.Business.Db.Entity
{
    [Table("log_properties")]
    public class LogPropertyEntity
    {
        [Key]
        public long Id { get; set; }
        public long LogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
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