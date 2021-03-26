using System;
using SimpleStack.Orm.Attributes;

namespace OffLogs.Business.Db.Entity
{
    [Alias("applications")]
    public class ApplicationEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [Alias("id")]
        public long Id { get; set; }
        
        [Alias("user_id")]
        [ForeignKey(typeof(UserEntity))]
        public long UserId { get; set; }
        
        [Alias("name")]
        public string Name { get; set; }
        
        [Alias("api_token")]
        public string ApiToken { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }
        
        [Alias("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}