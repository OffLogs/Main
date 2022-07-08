using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "user_emails")]
    public class UserEmailEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }

        [Property(NotNull = false)]
        [Column(Name = "email", Length = 200, NotNull = true)]
        public virtual string Email
        {
            get => _email;
            set
            {
                _email = value?.ToLower().Trim();
            }
        }
        
        [Property(NotNull = false)]
        [Column(Name = "verification_token", Length = 512, NotNull = false)]
        public virtual string VerificationToken { get; set; }

        [Property(NotNull = false, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "verification_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime? VerificationTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
        
        [Property(NotNull = true, TypeType = typeof(UtcDateTimeType))]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime UpdateTime { get; set; }

        [ManyToOne(
            ClassType = typeof(UserEntity), 
            Column = "user_id", 
            Lazy = Laziness.False,
            Cascade = "none"
        )]
        public virtual UserEntity User { get; set; }
        
        [Set(
            Table = "notification_rule_emails",
            Lazy = CollectionLazy.True,
            Cascade = "none",
            BatchSize = 20,
            Inverse = false
        )]
        [Key(
            Column = "user_email_id"
        )]
        [ManyToMany(
            Unique = true,
            ClassType = typeof(NotificationRuleEntity),
            Column = "notification_rule_id"
        )]
        public virtual ICollection<NotificationRuleEntity> NotificationRules { get; set; } = new List<NotificationRuleEntity>();
        
        public virtual bool IsVerified => VerificationTime.HasValue;

        private string _email;
        
        public UserEmailEntity() {}

        public virtual void SetUser(UserEntity user)
        {
            User = user;
            user.Emails.Add(this);
        }
    }
}
