using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Board.Statistic
{
    [Authorize]
    [Route("/board/[controller]")]
    [ApiController]
    public class StatisticController : MainApiControllerBase
    {
        public StatisticController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer,
            ILogger<StatisticController> logger
        ) : base(asyncRequestBuilder, commitPerformer, logger)
        {
        }

        [HttpPost("application")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(SearchRequest request)
            => this.RequestAsync().For<UsersListDto>().With(request);
    }
}
