using System;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Security;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "log_properties")]
    public class LogPropertyEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [JsonIgnore]
        [ManyToOne(
            ClassType = typeof(LogEntity), 
            Column = "log_id", 
            Lazy = Laziness.False,
            Cascade = "save-update"
        )]
        public virtual LogEntity Log { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "encrypted_key", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedKey { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "encrypted_value", SqlType = "bytea", NotNull = true)]
        public virtual byte[] EncryptedValue { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        #region NonFields

        public virtual string Key { get; set; }
        
        public virtual string Value { get; set; }

        #endregion 
        
        public LogPropertyEntity() {}

        public LogPropertyEntity(byte[] encryptedKey, byte[] encryptedValue)
        {
            EncryptedKey = encryptedKey;
            EncryptedValue = encryptedValue;
            CreateTime = DateTime.UtcNow;
        }
    }
}
