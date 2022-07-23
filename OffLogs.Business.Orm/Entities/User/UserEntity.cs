using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Constants.Monetization;
using OffLogs.Business.Orm.Entities.Notifications;

namespace OffLogs.Business.Orm.Entities.User
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
        
        [Bag(
            Inverse = true,
            Lazy = CollectionLazy.Extra,
            Cascade = "all-delete-orphan"
        )]
        [Key(Column = "user_id")]
        [OneToMany(ClassType = typeof(UserEmailEntity))]
        public virtual ICollection<UserEmailEntity> Emails { get; set; } = new List<UserEmailEntity>();
        
        [Bag(
            Inverse = true,
            Lazy = CollectionLazy.Extra,
            Cascade = "all-delete-orphan"
        )]
        [Key(Column = "user_id")]
        [OneToMany(ClassType = typeof(UserPaymentPackageEntity))]
        public virtual ICollection<UserPaymentPackageEntity> PaymentPackages { get; set; } = new List<UserPaymentPackageEntity>();
        
        [Bag(
            Inverse = true,
            Lazy = CollectionLazy.True,
            Cascade = "none"
        )]
        [Key(Column = "user_id")]
        [OneToMany(ClassType = typeof(NotificationRuleEntity))]
        public virtual ICollection<NotificationRuleEntity> NotificationRules { get; set; } = new List<NotificationRuleEntity>();
        
        public virtual bool IsVerified => VerificationTime.HasValue;
        
        public virtual UserPaymentPackageEntity LastPaymentPackage => PaymentPackages.MaxBy(
            item => item.CreateTime
        );

        public virtual PaymentPackageType ActivePaymentPackageType
        {
            get
            {
                var lastPackage = LastPaymentPackage;
                if (lastPackage == null || lastPackage.IsExpired)
                {
                    return PaymentPackageType.Basic;
                }

                return lastPackage.Type;
            }
        }

        public virtual UserPaymentPackageEntity PreviousPaymentPackage
        {
            get
            {
                if (PaymentPackages.Count >= 2)
                {
                    return PaymentPackages
                        .OrderByDescending(item => item.CreateTime)
                        .Skip(1)
                        .Take(1)
                        .FirstOrDefault();
                }

                return null;
            }
        }

        public UserEntity() {}

        public virtual void AddPaymentPackage(UserPaymentPackageEntity package)
        {
            package.User = this;
            PaymentPackages.Add(package);
        }
    }
}
