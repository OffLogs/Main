using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Constants;
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
    public class LogController : BaseApiController
    {
        private readonly IJwtAuthService _jwtService;
        private readonly ILogDao _logDao;
        private readonly IApplicationDao _applicationDao;
        
        public LogController(
            ILogger<LogController> logger, 
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
                
        [HttpPost("get")]
        public async Task<IActionResult> GetOne([FromBody]LogGetOneRequestModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                var log = await _logDao.GetLogAsync(model.Id);
                if (log == null)
                {
                    return JsonError(HttpStatusCode.NotFound);
                }
                if (!(await _applicationDao.IsOwner(userId, log.Application)))
                {
                    return JsonError(HttpStatusCode.Forbidden);
                }
                return JsonSuccess(log.GetResponseModel());
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
        
        [HttpPost("setFavorite")]
        public async Task<IActionResult> SetIsFavorite([FromBody]LogSetFavoriteRequestModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                if (!(await _logDao.IsOwner(userId, model.LogId)))
                {
                    return JsonError(HttpStatusCode.Forbidden);
                }
                await _logDao.SetIsFavoriteAsync(model.LogId, model.IsFavorite);
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