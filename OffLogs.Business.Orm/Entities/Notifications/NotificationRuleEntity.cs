using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Business.Orm.Entities.Notifications
{
    [Class(Table = "notification_rules")]
    public class NotificationRuleEntity: IEntity
    {
        [
            Id(Name = "Id", Generator = "native"), 
            Column(Name = "id", SqlType = "bigint", NotNull = true)
        ]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(UserEntity),
            Column = "user_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "none"
        )]
        public virtual UserEntity User { get; set; }
        
        [ManyToOne(
            ClassType = typeof(ApplicationEntity),
            Column = "application_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "none"
        )]
        public virtual ApplicationEntity Application { get; set; }
        
        [ManyToOne(
            ClassType = typeof(MessageTemplateEntity),
            Column = "notification_template_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "save-update"
        )]
        public virtual MessageTemplateEntity MessageTemplate { get; set; }
        
        [
            Property(NotNull = true),
            Column(Name = "title", Length = 512, NotNull = true)
        ]
        public virtual string Title { get; set; }
        
        [
            Property(NotNull = true),
            Column(Name = "notification_type_id", SqlType = "int", NotNull = true)
        ]
        public virtual NotificationType Type { get; set; }
        
        [
            Property(NotNull = true), 
            Column(Name = "logic_operator_type_id", SqlType = "int", NotNull = true)
        ]
        public virtual LogicOperatorType LogicOperator { get; set; }
        
        [
            Property(NotNull = true), 
            Column(Name = "period", SqlType = "int", NotNull = true)
        ]
        public virtual int Period { get; set; }
        
        [
            Property(NotNull = true, TypeType = typeof(UtcDateTimeType)),
            Column(Name = "last_execution_time", SqlType = "datetime", NotNull = true)
        ]
        public virtual DateTime LastExecutionTime { get; set; }
        
        [
            Property(NotNull = true), 
            Column(Name = "is_executing", SqlType = "boolean", NotNull = true)
        ]
        public virtual bool IsExecuting { get; set; }
        
        [
            Property(NotNull = true, TypeType = typeof(UtcDateTimeType)),
            Column(Name = "create_time", SqlType = "datetime", NotNull = true)
        ]
        public virtual DateTime CreateTime { get; set; }
        
        [
            Property(NotNull = true, TypeType = typeof(UtcDateTimeType)), 
            Column(Name = "update_time", SqlType = "datetime", NotNull = true)
        ]
        public virtual DateTime UpdateTime { get; set; }

        [
            Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan"), 
            Key(Column = "notification_rule_id"), 
            OneToMany(ClassType = typeof(NotificationConditionEntity))
        ]
        public virtual ICollection<NotificationConditionEntity> Conditions { get; set; } = new List<NotificationConditionEntity>();
        
        [Set(
            Table = "notification_rule_emails",
            Lazy = CollectionLazy.True,
            Cascade = "none",
            BatchSize = 20,
            Inverse = false
        )]
        [Key(
            Column = "notification_rule_id"
        )]
        [ManyToMany(
            Unique = true,
            ClassType = typeof(UserEmailEntity),
            Column = "user_email_id"
        )]
        public virtual ICollection<UserEmailEntity> Emails { get; set; } = new List<UserEmailEntity>();
        
        public NotificationRuleEntity() {}
        
        public virtual bool IsOwner(long userId)
        {
            return User.Id == userId;
        }
        
        public virtual void AddEmail(UserEmailEntity email)
        {
            Emails.Add(email);
        }
    }
}
