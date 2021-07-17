using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Kafka.Models
{
    public sealed record LogMessageModel
    {
        public string Token { get; set; }
        public string ApplicationJwtToken { get; set; }
        public string ClientIp { get; set; }
        
        public LogLevel LogLevel { get; set; }
        
        public string Message { get; set; }
        
        public DateTime LogTime { get; set; }

        public ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        public ICollection<LogPropertyEntity> Properties { get; set; } = new List<LogPropertyEntity>();

        public LogMessageModel() {}

        public LogMessageModel(string applicationJwt, LogEntity logEntity)
        {
            ApplicationJwtToken = applicationJwt;
            Token = logEntity.Token;
            LogLevel = logEntity.Level;
            Message = logEntity.Message;
            LogTime = logEntity.LogTime;
            Traces = logEntity.Traces;
            Properties = logEntity.Properties;
        }

        public LogEntity GetEntity()
        {
            var log = new LogEntity
            {
                Token = Token,
                Level = LogLevel,
                Message = Message,
                LogTime = LogTime
            };
            Traces?.ForEach(log.AddTrace);
            Properties?.ForEach(log.AddProperty);
            return log;
        }
    }
}