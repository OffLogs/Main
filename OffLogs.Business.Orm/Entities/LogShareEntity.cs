using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Utils;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "log_shares", NameType = typeof(LogShareEntity))]
    public class LogShareEntity : IEntity
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
            ClassType = typeof(LogEntity), 
            Column = "log_id", 
            Lazy = Laziness.Proxy,
            Fetch = FetchMode.Join
        )]
        public virtual LogEntity Log { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime CreateTime { get; set; }

        [Property(NotNull = true)]
        [Column(Name = "update_time", SqlType = "datetime", NotNull = false)]
        public virtual DateTime UpdateTime { get; set; }
    }
}