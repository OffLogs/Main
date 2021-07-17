using System;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Abstractions;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Frontend.Controllers
{
    public class FrontendApiControllerBase: ApiControllerBase
    {
        public FrontendApiControllerBase(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IExpectCommit commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }
        
        public override Func<Exception, IActionResult> Fail => ProcessFail;

        private static IActionResult ProcessFail(Exception exception)
        {
            if (exception is IDomainException)
                return new BadRequestObjectResult(new
                {
                    Type = exception.GetType().Name,
                    Message = exception.Message
                });

            throw exception;
        }
    }
}