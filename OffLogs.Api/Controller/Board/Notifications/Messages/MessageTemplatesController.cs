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
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Board.Notifications.Messages
{
    [Authorize]
    [Route("/board/notifications/message-templates")]
    [ApiController]
    public class MessageTemplatesController : MainApiControllerBase
    {
        public MessageTemplatesController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer,
            ILogger<MessageTemplatesController> logger
        ) : base(asyncRequestBuilder, commitPerformer, logger)
        {
        }

        [HttpPost("list")]
        [ProducesResponseType(typeof(ListDto<MessageTemplateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetMessagesList(GetMessageTemplateListRequest request)
            => this.RequestAsync().For<ListDto<MessageTemplateDto>>().With(request);
        
        [HttpPost("set")]
        [ProducesResponseType(typeof(MessageTemplateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> SetMessage(SetMessageTemplateRequest templateRequest)
            => this.RequestAsync().For<MessageTemplateDto>().With(templateRequest);
        
        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> DeleteMessage(DeleteMessageTemplateRequest request)
            => this.RequestAsync(request);
        
        
    }
}
