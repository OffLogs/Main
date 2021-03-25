using System;
using Dapper.Contrib.Extensions;

namespace OffLogs.Business.Db.Entity
{
    [Table("users")]
    public class UserEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        
        [Computed]
        public string Password { get; set; }
    }
}