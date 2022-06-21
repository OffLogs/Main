using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Board.UserEmail
{
    [Authorize]
    [Route("/board/user/email")]
    [ApiController]
    public class UserEmailController : MainApiControllerBase
    {
        public UserEmailController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer,
            ILogger<UserEmailController> logger
        ) : base(asyncRequestBuilder, commitPerformer, logger)
        {
        }

        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(SearchRequest request)
            => this.RequestAsync().For<UsersListDto>().With(request);
        
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Add(AddRequest request)
            => this.RequestAsync().For<UserEmailDto>().With(request);
        
        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Add(DeleteRequest request)
            => this.RequestAsync(request);
    }
}
