using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogRequestHandler : IAsyncRequestHandler<AddCommonLogsRequest>
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;

        public AddCommonLogRequestHandler(
            IKafkaProducerService kafkaProducerService,
            IHttpContextAccessor httpContextAccessor,
            IRequestService requestService,
            IThrottleRequestsService throttleRequestsService
        )
        {
            _kafkaProducerService = kafkaProducerService;
            _httpContextAccessor = httpContextAccessor;
            _requestService = requestService;
            _throttleRequestsService = throttleRequestsService;
        }

        public async Task ExecuteAsync(AddCommonLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThowExceptionAsync(
                RequestItemType.Application,
                _requestService.GetApplicationIdFromJwt()
            );

            var token = _requestService.GetApiToken();
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var applicationEncryptor = AsymmetricEncryptor.FromPublicKeyBytes(
                _requestService.GetApplicationPublicKeyFromJwt()    
            );
            var logSymmetricEncryptor = SymmetricEncryptor.GenerateKey();
            var encryptedSymmetricKey = applicationEncryptor.EncryptData(
                logSymmetricEncryptor.Key.GetKey()
            );
            
            foreach (var log in request.Logs)
            {
                var logEntity = new LogEntity()
                {
                    EncryptedSymmetricKey = encryptedSymmetricKey,
                    EncryptedMessage = logSymmetricEncryptor.EncryptData(log.Message),
                    Level = log.LogLevel,
                    LogTime = log.Timestamp
                };
                if (log.Properties != null)
                {
                    foreach (var property in log.Properties)
                    {
                        var encryptedKey = logSymmetricEncryptor.EncryptData(property.Key);
                        var encryptedValue = logSymmetricEncryptor.EncryptData(property.Value);
                        logEntity.AddProperty(new LogPropertyEntity(encryptedKey, encryptedValue));
                    }
                }
                if (log.Traces != null)
                {
                    foreach (var trace in log.Traces)
                    {
                        var encryptedTrace = logSymmetricEncryptor.EncryptData(trace);
                        logEntity.AddTrace(new LogTraceEntity(encryptedTrace));
                    }
                }
                await _kafkaProducerService.ProduceLogMessageAsync(token, logEntity, clientIp);
            }
        }
    }
}