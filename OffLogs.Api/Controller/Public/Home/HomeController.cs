using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Home;
using OffLogs.Business.Mvc.Attribute.Auth;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Public.Home
{
    [ApiController]
    [Route("/[controller]")]
    public class HomeController : MainApiControllerBase
    {
        public HomeController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }
        
        [HttpGet("/ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(PingRequest request)
            => this.RequestAsync().For<PongResponse>().With(request);

        [HttpGet("/application-auth-ping")]
        [OnlyAuthorizedApplication]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> ApplicationAuthPing(PingRequest request)
            => this.RequestAsync().For<PongResponse>().With(request);
    }
}