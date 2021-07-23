using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Business.Controller.Board.Log.Actions;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Business.Controller.Board.Log
{
    [Authorize]
    [Route("/board/[controller]")]
    [ApiController]
    public class LogController : MainApiControllerBase
    {
        public LogController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }

        [HttpPost("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> AddCommonLog(GetListRequest request)
            => this.RequestAsync().For<PaginatedListDto<LogListItemDto>>().With(request);
        
        [HttpPost("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetOne(GetRequest request)
            => this.RequestAsync().For<LogDto>().With(request);
        
        [HttpPost("setFavorite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> SetIsFavorite(SetIsFavoriteRequest request)
            => this.RequestAsync(request);

        [HttpPost("getStatisticForNow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> GetStatisticForNow(GetLogStatisticForNowRequest request)
            => this.RequestAsync()
                .For<LogStatisticForNowDto>()
                .With(request);
    }
}
