using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log;
using Persistence.Transactions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OffLogs.Api.Controller.Public.Log
{
    [Route("/[controller]")]
    [ApiController]
    public class LogController : MainApiControllerBase
    {
        public LogController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer,
            ILogger<LogController> logger
        ) : base(asyncRequestBuilder, commitPerformer, logger)
        {
        }

        // TODO: Remove it
        [NonAction]
        [HttpPost("getShared")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetOne(GetBySharedTokenRequest request)
            => this.RequestAsync().For<LogSharedDto>().With(request);
    }
}
