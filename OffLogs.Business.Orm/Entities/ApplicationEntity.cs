using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "applications")]
    public class ApplicationEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(UserEntity), 
            Column = "user_id", 
            Lazy = Laziness.False,
            Cascade = "delete-orphan"
        )]
        public virtual UserEntity User { get; set; }

        [Set(
            Table = "application_users",
            Lazy = CollectionLazy.True,
            Cascade = "none",
            BatchSize = 20
        )]
        [Key(
            Column = "application_id"
        )]
        [ManyToMany(
            Unique = true,
            ClassType = typeof(UserEntity),
            Column = "user_id"
        )]
        public virtual ICollection<UserEntity> SharedForUsers { get; set; } = new List<UserEntity>();

        [Property(NotNull = true)]
        [Column(Name = "name", Length = 200, NotNull = true)]
        public virtual string Name { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "api_token", Length = 2048, NotNull = true)]
        public virtual string ApiToken { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "public_key", SqlType = "bytea", NotNull = true)]
        public virtual byte[] PublicKey { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "encrypted_private_key", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedPrivateKey { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }
        
        public ApplicationEntity() {}

        public ApplicationEntity(long id, byte[] publicKey)
        {
            Id = id;
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        }
        
        public ApplicationEntity(UserEntity user, string name)
        {
            User = user;
            Name = name;
            ApiToken = "tempToken";
            CreateTime = DateTime.UtcNow;
            UpdateTime = DateTime.UtcNow;
        }
    }
}
