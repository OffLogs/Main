using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Public.User
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : MainApiControllerBase
    {
        public UserController(
            IAsyncRequestBuilder asyncRequestBuilder, 
            IDbSessionProvider commitPerformer
        ) : base(asyncRequestBuilder, commitPerformer)
        {
        }
        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> CheckIsLoggedIn([FromBody] LoginRequest request)
            => this.RequestAsync()
                .For<LoginResponseDto>()
                .With(request);

        [HttpGet("checkIsLoggedIn")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> CheckIsLoggedIn([FromQuery] CheckIsLoggedInRequest request)
            => this.RequestAsync(request);
        
        [HttpPost("registration/step1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> RegistrationStep1([FromBody] RegistrationStep1Request request)
            => this.RequestAsync(request);
        
        [HttpPost("registration/step2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> RegistrationStep2([FromBody] RegistrationStep2Request request)
            => this.RequestAsync()
                .For<RegistrationStep2ResponseDto>()
                .With(request);
    }
}