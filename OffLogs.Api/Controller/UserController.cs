using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Api.Models.Request.User;
using OffLogs.Api.Services.LogParser;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;
using OffLogs.Business.Mvc.Attribute.Auth;
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
    }
}