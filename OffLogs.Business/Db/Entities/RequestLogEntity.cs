using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Entity
{
    [Class(Table = "request_logs")]
    public class RequestLogEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "INT", NotNull = true)]
        public virtual long Id { get; set; }

        [Property(TypeType = typeof(RequestLogType), NotNull = true)]
        [Column(Name = "type", Length = 4, NotNull = true)]
        public virtual RequestLogType Type { get; set; }
        
        [Property(NotNull = false)]
        [Column(Name = "token", Length = 512, NotNull = false)]
        public virtual string Token { get; set; }
        
        [Property(NotNull = false)]
        [Column(Name = "client_ip", Length = 200, NotNull = false)]
        public virtual string ClientIp { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "data", NotNull = true)]
        public virtual string Data { get; set; }
        
        [Property(NotNull = true)]
        [Column(Name = "create_time", SqlType = "datetime", NotNull = true)]
        public virtual DateTime CreateTime { get; set; }
    }
}