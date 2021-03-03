using System;
using Dapper.Contrib.Extensions;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Entity
{
    [Table("LogTraces")]
    public class LogTraceEntity
    {
        [Key]
        public long Id { get; set; }
        public long LogId { get; set; }
        public string Trace { get; set; }
        public DateTime CreateTime { get; set; }
    }
}