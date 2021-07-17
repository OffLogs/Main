using System;
using Domain.Abstractions;
using Newtonsoft.Json;
using NHibernate.Mapping.Attributes;
using OffLogs.Business.Common.Constants;

namespace OffLogs.Business.Orm.Entities
{
    [Class(Table = "request_logs")]
    public class RequestLogEntity: IEntity
    {
        [Id(Name = "Id", Generator = "native")]
        [Column(Name = "id", SqlType = "bigint", NotNull = true)]
        public virtual long Id { get; set; }

        [Property(TypeType = typeof(RequestLogType), NotNull = true)]
        [Column(Name = "type", SqlType = "int", NotNull = true)]
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

        public RequestLogEntity() {}

        public RequestLogEntity(RequestLogType type, string clientIp, object data, string token)
        {
            Type = type;
            ClientIp = clientIp;
            Data = JsonConvert.SerializeObject(data);
            Token = token;
            CreateTime = DateTime.Now;
        }
    }
}