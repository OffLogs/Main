using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Orm.Entities.Notifications
{
    [Class(Table = "notification_message_templates")]
    public class MessageTemplateEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(UserEntity),
            Column = "user_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join
        )]
        public virtual UserEntity User { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "subject", Length = 1024, NotNull = true)]
        public virtual string Subject { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "body", NotNull = true)]
        public virtual string Body { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }
        
        public MessageTemplateEntity() {}

        public MessageTemplateEntity(string subject, string body)
        {
            Subject = subject;
            Body = body;
            CreateTime = DateTime.UtcNow;
            UpdateTime = DateTime.UtcNow;
        }
        
        public virtual bool IsOwner(long userId)
        {
            return User.Id == userId;
        }
    }
}
