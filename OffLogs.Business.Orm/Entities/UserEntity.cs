using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "users")]
    public class UserEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "user_name", Length = 200, NotNull = false)]
        public virtual string UserName { get; set; }
        
        [Property(NotNull = false)]
        [Column(Name = "email", Length = 200, NotNull = true)]
        public virtual string Email { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "public_key", SqlType = "bytea", NotNull = true)]
        public virtual byte[] PublicKey { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "signed_data", SqlType = "bytea", NotNull = true)]
        public virtual byte[] SignedData { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "sign", SqlType = "bytea", NotNull = true)]
        public virtual byte[] Sign { get; set; }
        
        [Property(NotNull = false)]
        [Column(Name = "verification_token", Length = 512, NotNull = false)]
        public virtual string VerificationToken { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "status", SqlType = "int", NotNull = true)]
        public virtual UserStatus Status { get; set; }

        [Property(NotNull = false, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "verification_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime? VerificationTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }

        [Set(
            Table = "log_favorites",
            Lazy = CollectionLazy.True,
            Cascade = "none",
            BatchSize = 20
       )]
        [Key(
           Column = "user_id"
       )]
        [ManyToMany(
           Unique = true,
           ClassType = typeof(LogEntity),
           Column = "log_id"
       )]
        public virtual ICollection<LogEntity> FavoriteLogs { get; set; } = new List<LogEntity>();
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "save-update")]
        [Key(Column = "user_id")]
        [OneToMany(ClassType = typeof(UserEmailEntity))]
        public virtual ICollection<UserEmailEntity> Emails { get; set; } = new List<UserEmailEntity>();
        
        public virtual bool IsVerificated => VerificationTime.HasValue;
        
        public UserEntity() {}
    }
}
