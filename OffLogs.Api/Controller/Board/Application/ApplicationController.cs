using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Controller.Board.Application.Actions;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using Persistence.Transactions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Application
{
    [Route("/board/[controller]")]
    [Authorize]
    [ApiController]
    public class ApplicationController : MainApiControllerBase
    {
        public ApplicationController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }

        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(GetListRequest request)
            => this.RequestAsync().For<PaginatedListDto<ApplicationListItemDto>>().With(request);

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Update(UpdateRequest request)
            => this.RequestAsync().For<ApplicationDto>().With(request);
    }
}
