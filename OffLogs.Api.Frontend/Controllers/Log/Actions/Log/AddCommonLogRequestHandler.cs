using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Api.Frontend.Services;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddCommonLogRequestHandler : IAsyncRequestHandler<AddCommonLogsRequest>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;
        private readonly ILogProducerService _logProducerService;

        public AddCommonLogRequestHandler(
            IHttpContextAccessor httpContextAccessor,
            IRequestService requestService,
            IThrottleRequestsService throttleRequestsService,
            ILogProducerService logProducerService
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _requestService = requestService;
            _throttleRequestsService = throttleRequestsService;
            _logProducerService = logProducerService;
        }

        public async Task ExecuteAsync(AddCommonLogsRequest request)
        {
             await _throttleRequestsService.CheckOrThrowExceptionByApplicationIdAsync(
                _requestService.GetApplicationIdFromJwt(),
                _requestService.GetUserIdFromJwt()
            );

            var application = new ApplicationEntity(
                _requestService.GetApplicationIdFromJwt(),
                _requestService.GetApplicationPublicKeyFromJwt()
            );
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            foreach (var log in request.Logs)
            {
                await _logProducerService.AddToKafkaAsync(
                    application,
                    log.Message,
                    log.LogLevel,
                    log.Timestamp,
                    log.Properties,
                    log.Traces,
                    clientIp
                );
            }
        }
    }
}
