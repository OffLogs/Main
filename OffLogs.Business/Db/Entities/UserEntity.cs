using System;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;

namespace OffLogs.Business.Db.Entities
{
    [Class(Table = "users")]
    public class UserEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "user_name", Length = 200, NotNull = true)]
        public virtual string UserName { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "email", Length = 200, NotNull = true)]
        public virtual string Email { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "password_hash", SqlType = "bytea", NotNull = true)]
        public virtual byte[] PasswordHash { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "password_salt", SqlType = "bytea", NotNull = true)]
        public virtual byte[] PasswordSalt { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }
        
        public virtual string Password { get; set; }
    }
}