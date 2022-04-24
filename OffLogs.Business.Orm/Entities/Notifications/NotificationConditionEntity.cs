using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Business.Orm.Entities.Notifications
{
    [Class(Table = "notification_conditions")]
    public class NotificationConditionEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }
        
        [ManyToOne(
            ClassType = typeof(NotificationRuleEntity),
            Column = "notification_rule_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "delete-orphan"
        )]
        public virtual NotificationRuleEntity Rule { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "field_type_id", SqlType = "int", NotNull = true)]
        public virtual ConditionFieldType ConditionField { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "value", Length = 512, NotNull = true)]
        public virtual string Value { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }

        public NotificationConditionEntity()
        {
        }
    }
}
