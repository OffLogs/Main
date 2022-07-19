using System;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using OffLogs.Business.Common.Constants.Monetization;

namespace OffLogs.Business.Orm.Entities.User
{
    [Class(Table = "user_payment_packages")]
    public class UserPaymentPackageEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "payment_package_type_id", SqlType = "int", NotNull = true)]
        public virtual PaymentPackageType Type { get; set; }

        [Property(NotNull = false, TypeType = typeof(DateTime))]
        [Column(Name = "expiration_date", SqlType = "date", NotNull = true)]
        public virtual DateTime ExpirationDate { get; set; }

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
        
        public virtual bool IsExpired => ExpirationDate < DateTime.UtcNow;

        public UserPaymentPackageEntity() {}

        public virtual void SetUser(UserEntity user)
        {
            User = user;
            user.PaymentPackages.Add(this);
        }
    }
}
