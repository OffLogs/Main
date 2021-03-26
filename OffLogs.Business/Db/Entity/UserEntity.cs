using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using SimpleStack.Orm.Attributes;

namespace OffLogs.Business.Db.Entity
{
    [Alias("users")]
    public class UserEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }
        
        [Alias("user_name")]
        public string UserName { get; set; }
        
        [Alias("email")]
        public string Email { get; set; }
        
        [Alias("password_hash")]
        public byte[] PasswordHash { get; set; }
        
        [Alias("password_salt")]
        public byte[] PasswordSalt { get; set; }
        
        [Alias("create_time")]
        public DateTime CreateTime { get; set; }
        
        [Alias("update_time")]
        public DateTime UpdateTime { get; set; }
        
        [Computed]
        [Ignore]
        public string Password { get; set; }
    }
}