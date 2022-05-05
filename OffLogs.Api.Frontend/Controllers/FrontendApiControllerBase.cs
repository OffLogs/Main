using System;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Abstractions;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Exceptions.Api;
using Persistence.Transactions.Behaviors;
using Serilog.Core;

namespace OffLogs.Api.Frontend.Controllers
{
    public class FrontendApiControllerBase: ApiControllerBase
    {
        protected ILogger<FrontendApiControllerBase> Logger { get; }

        public FrontendApiControllerBase(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer,
            ILogger<FrontendApiControllerBase> logger
        ) : base(asyncRequestBuilder, commitPerformer)
        {
            Logger = logger;
        }

        public FrontendApiControllerBase(
            IAsyncRequestBuilder asyncRequestBuilder,
            ILogger<FrontendApiControllerBase> logger
        ) : base(asyncRequestBuilder)
        {
            Logger = logger;
        }

        public override Func<Exception, IActionResult> Fail => ProcessFail;

        private IActionResult ProcessFail(Exception exception)
        {
            var message = exception?.Message;
            if (exception is not IDomainException)
            {
                Logger.LogError(exception, exception?.Message);
                exception = new ServerException();
            }

            return new BadRequestObjectResult(new
            {
                Type = exception.GetType().Name,
                Message = exception.Message
            });
        }
    }
}
