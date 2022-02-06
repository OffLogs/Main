using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Kafka.Models
{
    public sealed record LogMessageDto: IKafkaDto
    {
        public string Token { get; set; }
        public long ApplicationId { get; set; }
        public string ClientIp { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string EncryptedSymmetricKey { get; set; }
        public DateTimeOffset LogTime { get; set; }
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public ICollection<string> Traces { get; set; } = new List<string>();

        public LogMessageDto() {}

        public LogMessageDto(LogEntity logEntity)
        {
            if (logEntity.Application == null)
                throw new ArgumentNullException(nameof(logEntity.Application));
            
            ApplicationId = logEntity.Application.Id;
            Token = logEntity.Token;
            LogLevel = logEntity.Level;
            Message = Convert.ToBase64String(logEntity.EncryptedMessage);
            EncryptedSymmetricKey = Convert.ToBase64String(logEntity.EncryptedSymmetricKey);
            LogTime = logEntity.LogTime;
            Traces = logEntity.Traces.Select(
                encryptedTrace => Convert.ToBase64String(encryptedTrace.EncryptedTrace)
            ).ToList();
            Properties = logEntity.Properties.ToDictionary(
                property => Convert.ToBase64String(property.EncryptedKey),
                property => Convert.ToBase64String(property.EncryptedValue)
            );
        }

        public LogEntity GetEntity()
        {
            var log = new LogEntity
            {
                Token = Token,
                Level = LogLevel,
                EncryptedMessage = Convert.FromBase64String(Message),
                EncryptedSymmetricKey = Convert.FromBase64String(EncryptedSymmetricKey),
                LogTime = LogTime,
                CreateTime = DateTime.UtcNow,
                Application = new ApplicationEntity()
                {
                    Id = ApplicationId
                }
            };
            EnumerableExtensions.ForEach(Traces?
                .Select(
                    trace => new LogTraceEntity(
                        Convert.FromBase64String(trace)    
                    )
                ), 
                log.AddTrace
            );
            EnumerableExtensions.ForEach(Properties?
                .Select(
                    property => new LogPropertyEntity(
                        Convert.FromBase64String(property.Key),
                        Convert.FromBase64String(property.Value)
                    )
                ), 
                log.AddProperty
            );
            return log;
        }
    }
}
