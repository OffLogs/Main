using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Business.Orm.Entities.Notifications
{
    [Class(Table = "notification_rules")]
    public class NotificationRuleEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(UserEntity),
            Column = "user_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "delete-orphan"
        )]
        public virtual UserEntity User { get; set; }
        
        [ManyToOne(
            ClassType = typeof(ApplicationEntity),
            Column = "application_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "delete-orphan"
        )]
        public virtual ApplicationEntity Application { get; set; }
        
        [ManyToOne(
            ClassType = typeof(NotificationMessageEntity),
            Column = "notification_message_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "delete-orphan"
        )]
        public virtual NotificationMessageEntity Message { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "notification_type_id", SqlType = "int", NotNull = true)]
        public virtual NotificationType Type { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "logic_operator_type_id", SqlType = "int", NotNull = true)]
        public virtual LogicOperatorType LogicOperator { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "period", SqlType = "int", NotNull = true)]
        public virtual long Period { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "last_execution_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime LastExecutionTime { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "is_executing", SqlType = "boolean", NotNull = true)]
        public virtual bool IsExecuting { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }
        
        [Bag(Inverse = true, Lazy = CollectionLazy.Extra, Cascade = "all-delete-orphan")]
        [Key(Column = "notification_rule_id")]
        [OneToMany(ClassType = typeof(LogTraceEntity))]
        public virtual ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        public NotificationRuleEntity() {}
    }
}
