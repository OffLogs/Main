using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "logs", NameType = typeof(LogEntity))]
    public class LogEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        private string _token { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "token", Length = 1024, NotNull = true)]
        public virtual string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token))
                {
                    _token = SecurityUtil.GetTimeBasedToken();
                }
                return _token;
            }
            set => _token = value;
        }
        
        [JsonIgnore]
        [ManyToOne(
            ClassType = typeof(ApplicationEntity), 
            Column = "application_id", 
            Lazy = Laziness.False,
            Fetch = FetchMode.Join
        )]
        public virtual ApplicationEntity Application { get; set; }

        [Property(TypeType = typeof(LogLevel), NotNull = true)]
        [Column(Name = "level", SqlType = "int", NotNull = true)]
        public virtual LogLevel Level { get; set; }
               
        [Property(NotNull = true)]
        [Column(Name = "encrypted_message", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedMessage { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "encrypted_symmetric_key", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedSymmetricKey { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "log_time", SqlType = "datetime")]
        public virtual DateTime LogTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime CreateTime { get; set; }
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogTraceEntity))]
        public virtual ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogPropertyEntity))]
        public virtual ICollection<LogPropertyEntity> Properties { get; set; } = new List<LogPropertyEntity>();

        [JsonIgnore]
        [Set(
            Table = "log_favorites",
            Lazy = CollectionLazy.True,
            Cascade = "delete-orphan",
            BatchSize = 20
       )]
        [Key(
           Column = "log_id"
       )]
        [ManyToMany(
           Unique = true,
           ClassType = typeof(UserEntity),
           Column = "user_id"
       )]
        public virtual ICollection<UserEntity> FavoriteForUsers { get; set; } = new List<UserEntity>();

        [JsonIgnore]
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "log_id")]
        [OneToMany(ClassType = typeof(LogShareEntity))]
        public virtual ICollection<LogShareEntity> LogShares { get; set; } = new List<LogShareEntity>();

        public virtual bool IsFavorite { get; set; }

        public virtual string Message { get; set; }
        
        public virtual void AddTrace(LogTraceEntity entity)
        {
            entity.Log = this;
            Traces.Add(entity);
        }
        
        public virtual void AddProperty(LogPropertyEntity entity)
        {
            entity.Log = this;
            Properties.Add(entity);
        }
    }
}
