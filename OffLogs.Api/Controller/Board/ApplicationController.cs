using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Controller.Board
{
    [Authorize]
    [Route("/board/[controller]")]
    [ApiController]
    public class ApplicationController : BaseApiController
    {
        private readonly IJwtAuthService _jwtService;
        private readonly ILogDao _logDao;
        private readonly IApplicationDao _applicationDao;
        
        public ApplicationController(
            ILogger<ApplicationController> logger, 
            IConfiguration configuration,
            ILogDao logDao,
            IApplicationDao applicationDao,
            IJwtAuthService jwtService
        ) : base(logger, configuration)
        {
            _logDao = logDao;
            _applicationDao = applicationDao;
            _jwtService = jwtService;
        }
        
        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody]LogListRequestModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                return JsonSuccess();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
    }
}