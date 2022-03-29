using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using NHibernate.Mapping;
using OffLogs.Api.Frontend.Services;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Http.ThrottleRequests;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddSerilogLogRequestHandler : IAsyncRequestHandler<AddSerilogLogsRequest>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;
        private readonly ILogProducerService _logProducerService;

        public AddSerilogLogRequestHandler(
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

        public async Task ExecuteAsync(AddSerilogLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThrowExceptionAsync(
                RequestItemType.Application,
                _requestService.GetApplicationIdFromJwt()
            );

            var application = new ApplicationEntity(
                _requestService.GetApplicationIdFromJwt(),
                _requestService.GetApplicationPublicKeyFromJwt()
            );

            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            foreach (var log in request.Events)
            {
                var traces = log.Exception?.Split("\n");
                await _logProducerService.AddToKafkaAsync(
                    application,
                    log.RenderedMessage,
                    log.LogLevel.ToLogLevel(),
                    log.Timestamp,
                    log.Properties,
                    traces,
                    clientIp
                );
            }
        }
    }
}
