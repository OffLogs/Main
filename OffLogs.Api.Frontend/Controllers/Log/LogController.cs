﻿using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Frontend.Controllers.Log.Actions.Log;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Frontend.Controllers.Log
{
    [Route("/[controller]")]
    public class LogController : FrontendApiControllerBase
    {
        public LogController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IExpectCommit commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }
        
        [HttpPost]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog([FromBody] AddCommonLogsRequest request)
            => this.RequestAsync(request);
    }
}