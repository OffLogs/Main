using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offlogs.Business.Api.Controller.Board.User.Actions;
using Offlogs.Business.Api.Controller.Board.User.Dto;
using Persistence.Transactions.Behaviors;

namespace Offlogs.Business.Api.Controller.Board.User
{
    [Authorize]
    [Route("/board/[controller]")]
    [ApiController]
    public class UserController : MainApiControllerBase
    {
        public UserController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }

        [HttpPost("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(SearchRequest request)
            => this.RequestAsync().For<SearchResponseDto>().With(request);
    }
}
