using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offlogs.Business.Api.Controller.Board.Application.Actions;
using Offlogs.Business.Api.Dto;
using Offlogs.Business.Api.Dto.Entities;
using Persistence.Transactions.Behaviors;

namespace Offlogs.Business.Api.Controller.Board.Application
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

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Add(AddRequest request)
            => this.RequestAsync().For<ApplicationDto>().With(request);

        [HttpPost("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetOne(GetRequest request)
            => this.RequestAsync().For<ApplicationDto>().With(request);
    }
}
