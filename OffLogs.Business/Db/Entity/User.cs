using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OffLogs.Business.Db.Entity
{
    [Table("Users")]
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        
        public List<LogEntity> Logs { get; set; }
    }
}