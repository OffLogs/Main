using System;
using Dapper.Contrib.Extensions;

namespace OffLogs.Business.Db.Entity
{
    [Table("LogProperties")]
    public class LogPropertyEntity
    {
        [Key]
        public long Id { get; set; }
        public long LogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateTime { get; set; }
        
        public User User { get; set; }
    }
}