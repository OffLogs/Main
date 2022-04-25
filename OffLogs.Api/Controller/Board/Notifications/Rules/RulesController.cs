using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Settings.NotificationMessage;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Board.Notifications.Rules
{
    [Authorize]
    [Route("/board/notifications/[controller]")]
    [ApiController]
    public class RulesController : MainApiControllerBase
    {
        public RulesController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer,
            ILogger<RulesController> logger
        ) : base(asyncRequestBuilder, commitPerformer, logger)
        {
        }

        [HttpPost("set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> SetMessage(SetMessageRequest request)
            => this.RequestAsync().For<NotificationMessageDto>().With(request);
    }
}
