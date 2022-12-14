using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Frontend.Controllers.Log.Actions.Log;
using OffLogs.Business.Mvc.Attribute.Auth;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Frontend.Controllers.Log
{
    [Route("/[controller]")]
    public class LogController : FrontendApiControllerBase
    {
        public LogController(
            IAsyncRequestBuilder asyncRequestBuilder,
            ILogger<LogController> logger
        ) 
            : base(asyncRequestBuilder, logger)
        {
        }
        
        [HttpPost]
        [OnlyAuthorizedApplication]
        [Route("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog([FromBody] AddCommonLogsRequest request)
            => this.RequestAsync(request);
        
        [HttpPost]
        [OnlyAuthorizedApplication]
        [Route("add/serilog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddSerilogLog([FromBody] AddSerilogLogsRequest request)
            => this.RequestAsync(request);
        
        [HttpPost]
        [OnlyAuthorizedApplication]
        [Route("add/test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddTestLog([FromBody] AddTestLogsRequest request)
            => this.RequestAsync(request);
    }
}
