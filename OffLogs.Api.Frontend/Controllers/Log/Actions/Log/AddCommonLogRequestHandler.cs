using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogRequestHandler : IAsyncRequestHandler<AddCommonLogsRequest>
    {
        private readonly IAsyncQueryBuilder _asyncQueryBuilder;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IJwtApplicationService _jwtApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddCommonLogRequestHandler(
            IAsyncQueryBuilder asyncQueryBuilder,
            IKafkaProducerService kafkaProducerService,
            IJwtApplicationService jwtApplicationService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _asyncQueryBuilder = asyncQueryBuilder ?? throw new ArgumentNullException(nameof(asyncQueryBuilder));
            _kafkaProducerService = kafkaProducerService;
            _jwtApplicationService = jwtApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(AddCommonLogsRequest request)
        {
            var token = _jwtApplicationService.GetToken();
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