using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.Log
{
    public class LogService: ILogService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtApplicationService _jwtService;
        private readonly IKafkaProducerService _kafkaProducerService;

        public LogService(
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder queryBuilder,
            IJwtApplicationService jwtService,
            IKafkaProducerService kafkaProducerService
        )
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
            _jwtService = jwtService;
            _kafkaProducerService = kafkaProducerService;
        }
        
        public async Task<LogEntity> AddAsync(LogEntity log)
        {
            var properties = log.Properties.ToList();
            log.Properties = new List<LogPropertyEntity>();
            var traces = log.Traces.ToList();
            log.Traces = new List<LogTraceEntity>();
                
            await _commandBuilder.SaveAsync(log);

            properties.ForEach(log.AddProperty);
            traces.ForEach(log.AddTrace);
            return log;
        }

        public async Task<LogEntity> AddAsync(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            IDictionary<string, object> properties = null,
            ICollection<string> traces = null
        )
        {
            var log = await AssembleLog(
                application,
                message,
                level,
                timestamp,
                properties,
                traces
            );
            await _commandBuilder.SaveAsync(log);
            return log;
        }
        
        public async Task<LogEntity> AddToKafkaAsync(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            IDictionary<string, object> properties = null,
            ICollection<string> traces = null
        )
        {
            var log = await AssembleLog(
                application,
                message,
                level,
                timestamp,
                properties,
                traces
            );
            await _kafkaProducerService.ProduceLogMessageAsync(log);
            return log;
        }
        
        public async Task<bool> SetIsFavoriteAsync(long userId, long logId, bool isFavorite)
        {
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(logId);
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var isAlreadyFavorite = await _queryBuilder.For<bool>().WithAsync(
                new LogIsFavoriteCriteria(userId, logId)
            );
            if (isAlreadyFavorite)
            {
                return false;
            }
            user.FavoriteLogs.Add(log);
            await _commandBuilder.SaveAsync(user);
            return true;
        }

        private Task<LogEntity> AssembleLog(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            IDictionary<string, object> properties = null,
            ICollection<string> traces = null    
        )
        {
            var applicationEncryptor = AsymmetricEncryptor.FromPublicKeyBytes(application.PublicKey);
            var logSymmetricEncryptor = SymmetricEncryptor.GenerateKey();
            var encryptedSymmetricKey = applicationEncryptor.EncryptData(
                logSymmetricEncryptor.Key.GetKey()
            );
            
            var log = new LogEntity()
            {
                Application = application,
                EncryptedSymmetricKey = encryptedSymmetricKey,
                EncryptedMessage = logSymmetricEncryptor.EncryptData(message),
                Level = level,
                LogTime = timestamp
            };
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var encryptedKey = logSymmetricEncryptor.EncryptData(property.Key);
                    var encryptedValue = logSymmetricEncryptor.EncryptData(
                        property.Value?.GetAsJson()
                    );
                    log.AddProperty(
                        new LogPropertyEntity(encryptedKey, encryptedValue)    
                    );
                }
            }
            if (traces != null)
            {
                foreach (var trace in traces)
                {
                    var encryptedTrace = logSymmetricEncryptor.EncryptData(trace);
                    log.AddTrace(new LogTraceEntity(encryptedTrace));
                }
            }

            return Task.FromResult(log);
        }
    }
}