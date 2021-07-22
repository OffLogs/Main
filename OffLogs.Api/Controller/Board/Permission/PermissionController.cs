using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Controller.Board.Log.Actions;
using OffLogs.Api.Controller.Board.Permission.Actions;
using OffLogs.Api.Controller.Board.User.Actions;
using OffLogs.Api.Controller.Board.User.Dto;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using Persistence.Transactions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Permission
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
