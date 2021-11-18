using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using NHibernate.Mapping;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Extensions;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddSerilogLogRequestHandler : IAsyncRequestHandler<AddSerilogLogsRequest>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;
        private readonly ILogService _logService;

        public AddSerilogLogRequestHandler(
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

        public async Task ExecuteAsync(AddSerilogLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThowExceptionAsync(
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
                await _logService.AddToKafkaAsync(
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
