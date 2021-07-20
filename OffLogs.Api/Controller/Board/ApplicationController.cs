using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request;
using OffLogs.Business.Common.Models.Api.Request;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Dao;
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
        
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ApplicationAddModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                var application = await _applicationDao.CreateNewApplication(userId, model.Name);
                return JsonSuccess(
                    application.ResponseModel    
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
               
        [HttpPost("get")]
        public async Task<IActionResult> GetOne([FromBody]ApplicationGetModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                var application = await _applicationDao.GetAsync(model.Id);
                if (!await _applicationDao.IsOwner(userId, application))
                {
                    return JsonError(HttpStatusCode.Forbidden);
                }
                return JsonSuccess(application.ResponseModel);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
    }
}