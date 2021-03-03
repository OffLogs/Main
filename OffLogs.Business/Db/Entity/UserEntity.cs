using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OffLogs.Business.Db.Entity
{
    [Table("Users")]
    public class UserEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}