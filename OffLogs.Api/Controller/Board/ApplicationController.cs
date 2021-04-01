using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request;
using OffLogs.Api.Models.Request.Board;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Models.Response.Board;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
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
        public async Task<IActionResult> GetList([FromBody]PaginatedRequestModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                var (list, totalItems) = await _applicationDao.GetList(
                    userId,
                    model.Page
                );
                var responseList = list.Select(item => new ApplicationResponseModel(item)).ToList();
                return JsonSuccess(
                    new PaginatedResponseModel<ApplicationResponseModel>(responseList, totalItems)    
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ApplicationAddModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                var application = await _applicationDao.CreateNewApplication(userId, model.Name);
                return JsonSuccess(
                    new ApplicationResponseModel(application)    
                );
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
    }
}