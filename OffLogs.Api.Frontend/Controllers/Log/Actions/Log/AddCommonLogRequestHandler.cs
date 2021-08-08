using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
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
            
            foreach (var log in request.Logs)
            {
                var logEntity = new LogEntity()
                {
                    Message = log.Message,
                    Level = log.LogLevel,
                    LogTime = log.Timestamp
                };
                if (log.Properties != null)
                {
                    foreach (var property in log.Properties)
                    {
                        logEntity.AddProperty(new LogPropertyEntity(property.Key, property.Value));
                    }
                }
                if (log.Traces != null)
                {
                    foreach (var trace in log.Traces)
                    {
                        logEntity.AddTrace(new LogTraceEntity(trace));
                    }
                }
                await _kafkaProducerService.ProduceLogMessageAsync(token, logEntity, clientIp);
            }
        }
    }
}