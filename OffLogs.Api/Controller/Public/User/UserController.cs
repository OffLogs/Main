using System.Net;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using AspNetCore.ApiControllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Controller.Public.Home.Actions;
using OffLogs.Api.Controller.Public.User.Actions;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Dao;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Jwt;
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
        
        // [HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        // {
        //     var existsUser = await _userDao.GetByUserName(model.UserName);
        //     if (existsUser == null)
        //     {
        //         return JsonError(HttpStatusCode.Unauthorized);
        //     }
        //
        //     var passwordHash = SecurityUtil.GeneratePasswordHash(model.Password, existsUser.PasswordSalt);
        //     if (!passwordHash.CompareTo(existsUser.PasswordHash))
        //     {
        //         return JsonError(HttpStatusCode.Unauthorized);
        //     }
        //     return JsonSuccess(new LoginResponseModel
        //     {
        //         Token = _jwtService.BuildJwt(existsUser.Id)
        //     });
        // }
        
        [HttpGet("checkIsLoggedIn")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> CheckIsLoggedIn([FromQuery] CheckIsLoggedInRequest request)
            => this.RequestAsync(request);
    }
}