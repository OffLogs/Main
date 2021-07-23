using System;
using System.Collections.Generic;
using Domain.Abstractions;
using NHibernate.Mapping.Attributes;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "log_favorites")]
    public class FavoriteLogEntity : IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }

        [ManyToOne(
            ClassType = typeof(LogEntity),
            Column = "log_id",
            Lazy = Laziness.False,
            Fetch = FetchMode.Join,
            Cascade = "save-update"
        )]
        public virtual LogEntity Log { get; set; }

        [ManyToOne(
            ClassType = typeof(UserEntity),
            Column = "user_id",
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join,
            Cascade = "save-update"
        )]
        public virtual UserEntity User { get; set; }

        public FavoriteLogEntity() {}
    }
}