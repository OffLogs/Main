using System.ComponentModel.DataAnnotations.Schema;

namespace OffLogs.Business.Db.Entity
{
    [Table("Logs")]
    public class Log
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        
        public User Owner { get; set; }
    }
}