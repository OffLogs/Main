using System;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Abstractions;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller
{
    public class MainApiControllerBase: ApiControllerBase
    {
        protected readonly ILogger<MainApiControllerBase> Logger;

        public MainApiControllerBase(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer,
            ILogger<MainApiControllerBase> logger
        ) : base(asyncRequestBuilder, commitPerformer)
        {
            Logger = logger;
        }
        
        public override Func<Exception, IActionResult> Fail => ProcessFail;

        private IActionResult ProcessFail(Exception exception)
        {
            if (exception is not IDomainException)
            {
                Logger.LogError(exception?.Message, exception);
            }

            return new BadRequestObjectResult(new BadResponseDto
            {
                Message = exception.Message,
                Type = exception.GetType().Name
            });
        }
    }
}
