using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.UserEmail;
using Persistence.Transactions.Behaviors;

namespace OffLogs.Api.Controller.Public.UserEmail
{
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
        
        [HttpGet("/user/email/verify/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> CheckIsLoggedIn([FromRoute] VerificationRequest request)
            => this.RequestAsync(request);
    }
}
