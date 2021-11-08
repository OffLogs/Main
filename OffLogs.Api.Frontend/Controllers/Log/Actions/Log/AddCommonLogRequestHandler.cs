using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Log;
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
        private readonly ILogService _logService;

        public AddCommonLogRequestHandler(
            IHttpContextAccessor httpContextAccessor,
            IRequestService requestService,
            IThrottleRequestsService throttleRequestsService,
            ILogService logService
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _requestService = requestService;
            _throttleRequestsService = throttleRequestsService;
            _logService = logService;
        }

        public async Task ExecuteAsync(AddCommonLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThowExceptionAsync(
                RequestItemType.Application,
                _requestService.GetApplicationIdFromJwt()
            );

            var application = new ApplicationEntity(
                _requestService.GetApplicationIdFromJwt()
            );
            var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            foreach (var log in request.Logs)
            {
                await _logService.AddToKafkaAsync(
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