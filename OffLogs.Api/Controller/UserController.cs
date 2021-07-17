using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Models.Api.Request.User;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Dao;
using OffLogs.Business.Helpers;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Controller
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserDao _userDao;
        private readonly IJwtAuthService _jwtService;
        
        public UserController(
            ILogger<UserController> logger, 
            IConfiguration configuration,
            IUserDao userDao,
            IJwtAuthService jwtService
        ) : base(logger, configuration)
        {
            _userDao = userDao;
            _jwtService = jwtService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        {
            var existsUser = await _userDao.GetByUserName(model.UserName);
            if (existsUser == null)
            {
                return JsonError(HttpStatusCode.Unauthorized);
            }

            var passwordHash = SecurityUtil.GeneratePasswordHash(model.Password, existsUser.PasswordSalt);
            if (!passwordHash.CompareTo(existsUser.PasswordHash))
            {
                return JsonError(HttpStatusCode.Unauthorized);
            }
            return JsonSuccess(new LoginResponseModel
            {
                Token = _jwtService.BuildJwt(existsUser.Id)
            });
        }

        [Authorize]
        [HttpGet("checkIsLoggedIn")]
        public IActionResult CheckIsLoggedIn()
        {
            return JsonSuccess();
        }
    }
}