using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddSerilogLogRequestHandler : IAsyncRequestHandler<AddSerilogLogsRequest>
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;

        public AddSerilogLogRequestHandler(
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

        public async Task ExecuteAsync(AddSerilogLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThowExceptionAsync(
                RequestItemType.Application,
                _requestService.GetApplicationIdFromJwt()
            );

            var applicationEncryptor = AsymmetricEncryptor.FromPublicKeyBytes(
                _requestService.GetApplicationPublicKeyFromJwt()    
            );
            var logSymmetricEncryptor = SymmetricEncryptor.GenerateKey();
            var encryptedSymmetricKey = applicationEncryptor.EncryptData(
                logSymmetricEncryptor.Key.GetKey()
            );
            
            var token = _requestService.GetApiToken();
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            foreach (var log in request.Events)
            {
                var logEntity = new LogEntity()
                {
                    EncryptedSymmetricKey = encryptedSymmetricKey,
                    EncryptedMessage = logSymmetricEncryptor.EncryptData(log.RenderedMessage),
                    Level = log.LogLevel.ToLogLevel(),
                    LogTime = log.Timestamp
                };
                if (log.Properties != null)
                {
                    foreach (var property in log.Properties)
                    {
                        var encryptedKey = logSymmetricEncryptor.EncryptData(property.Key);
                        var encryptedValue = logSymmetricEncryptor.EncryptData(
                            property.Value?.GetAsJson()
                        );
                        logEntity.AddProperty(new LogPropertyEntity(encryptedKey, encryptedValue));
                    }
                }

                var traces = log.Exception?.Split("\n");
                if (traces != null)
                {
                    foreach (var trace in traces)
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