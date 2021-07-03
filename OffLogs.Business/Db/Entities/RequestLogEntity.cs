using System;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Types;

namespace OffLogs.Business.Db.Entities
{
    [Class(Table = "request_logs")]
    public class RequestLogEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "INT", NotNull = true)]
        public virtual long Id { get; set; }

        [Property(TypeType = typeof(RequestLogTypeConstantType), NotNull = true)]
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