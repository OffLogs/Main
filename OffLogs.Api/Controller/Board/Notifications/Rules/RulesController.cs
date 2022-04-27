using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Message;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule;
using Persistence.Transactions.Behaviors;
using GetListRequest = OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule.GetListRequest;

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

        [HttpPost("list")]
        [ProducesResponseType(typeof(ListDto<NotificationRuleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetList(GetListRequest request)
            => this.RequestAsync().For<ListDto<NotificationRuleDto>>().With(request);
        
        [HttpPost("set")]
        [ProducesResponseType(typeof(NotificationRuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> SetMessage(SetRuleRequest request)
            => this.RequestAsync().For<NotificationRuleDto>().With(request);
    }
}
