using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Permission;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Business.Controller.Board.Permission
{
    [Authorize]
    [Route("/board/[controller]")]
    [ApiController]
    public class PermissionController : MainApiControllerBase
    {
        public PermissionController(
            IAsyncRequestBuilder asyncRequestBuilder,
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }

        [HttpPost("addAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddAccess(AddAccessRequest request)
            => this.RequestAsync(request);
    }
}
