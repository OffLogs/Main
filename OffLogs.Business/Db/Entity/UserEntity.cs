using System;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Db.Entity
{
    public class UserEntity
    {
        public virtual long Id { get; set; }
        public virtual CityCode City { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual byte[] PasswordHash { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime UpdateTime { get; set; }
        
        public virtual string Password { get; set; }
    }
}