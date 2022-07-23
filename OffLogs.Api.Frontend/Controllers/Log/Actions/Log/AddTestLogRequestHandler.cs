using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Http.ThrottleRequests;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Frontend.Controllers.Log.Actions.Log
{
    public class AddTestLogRequestHandler : IAsyncRequestHandler<AddTestLogsRequest>
    {
        private readonly IRequestService _requestService;
        private readonly IThrottleRequestsService _throttleRequestsService;
        private readonly ILogger<AddTestLogRequestHandler> _logger;

        public AddTestLogRequestHandler(
            IRequestService requestService,
            IThrottleRequestsService throttleRequestsService,
            ILogger<AddTestLogRequestHandler> logger
        )
        {
            _requestService = requestService;
            _throttleRequestsService = throttleRequestsService;
            _logger = logger;
        }

        public async Task ExecuteAsync(AddTestLogsRequest request)
        {
            await _throttleRequestsService.CheckOrThrowExceptionByApplicationIdAsync(
                _requestService.GetApplicationIdFromJwt(),
                _requestService.GetUserIdFromJwt()
            );

            var application = new ApplicationEntity(
                _requestService.GetApplicationIdFromJwt(),
                _requestService.GetApplicationPublicKeyFromJwt()
            );
            _logger.LogTrace($"Test log received. Application: ${application.Id}. Count: ${request.Logs.Count}");
        }
    }
}
