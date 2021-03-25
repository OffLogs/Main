using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OffLogs.Business.Db.Entity
{
    [Table("applications")]
    public class ApplicationEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string ApiToken { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}