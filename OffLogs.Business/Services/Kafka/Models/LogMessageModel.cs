﻿using System;
using System.Collections.Generic;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Kafka.Models
{
    public sealed record LogMessageModel
    {
        public string Token { get; set; }
        public string ApplicationJwtToken { get; set; }
        public string ClientIp { get; set; }
        
        public string LogLevel { get; set; }
        
        public string Message { get; set; }
        
        public DateTime LogTime { get; set; }

        public ICollection<LogTraceEntity> Traces { get; set; } = new List<LogTraceEntity>();
        
        public ICollection<LogPropertyEntity> Properties { get; set; } = new List<LogPropertyEntity>();

        public LogMessageModel() {}

        public LogMessageModel(string applicationJwt, LogEntity logEntity)
        {
            ApplicationJwtToken = applicationJwt;
            Token = logEntity.Token;
            LogLevel = logEntity.Level.GetValue();
            Message = logEntity.Message;
            LogTime = logEntity.LogTime;
            Traces = logEntity.Traces;
            Properties = logEntity.Properties;
        }

        public LogEntity GetEntity()
        {
            return new()
            {
                Token = Token,
                Level = new LogLevel().FromString(LogLevel),
                Message = Message,
                LogTime = LogTime,
                Traces = Traces,
                Properties = Properties,
            };
        }
    }
}