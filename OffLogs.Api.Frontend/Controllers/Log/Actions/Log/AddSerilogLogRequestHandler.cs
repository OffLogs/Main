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
                _requestService.GetApplicationIdFromJwt()
            );

            var token = _requestService.GetApiToken();
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            foreach (var log in request.Events)
            {
                var logEntity = new LogEntity()
                {
                    Message = log.RenderedMessage,
                    Level = log.LogLevel.ToLogLevel(),
                    LogTime = log.Timestamp
                };
                if (log.Properties != null)
                {
                    foreach (var property in log.Properties)
                    {
                        logEntity.AddProperty(new LogPropertyEntity(property.Key, property.Value));
                    }
                }

                var traces = log.Exception?.Split("\n");
                if (traces != null)
                {
                    foreach (var trace in traces)
                    {
                        logEntity.AddTrace(new LogTraceEntity(trace));
                    }
                }
                await _kafkaProducerService.ProduceLogMessageAsync(token, logEntity, clientIp); 
            }
        }
    }
}