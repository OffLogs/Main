using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Response;
using OffLogs.Api.Models.Response.Board;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Models.Api.Request.Board;
using OffLogs.Business.Common.Models.Api.Response;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Dao;
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
        
        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody]LogListRequestModel model)
        {
            try
            {
                var userId = _jwtService.GetUserId();
                if (!(await _applicationDao.IsOwner(userId, model.ApplicationId)))
                {
                    return JsonError(HttpStatusCode.Forbidden);
                }
                var (list, totalItems) = await _logDao.GetList(
                    model.ApplicationId,
                    model.Page,
                    GlobalConstants.ListPageSize
                );
                var responseList = list.Select(item => new LogResponseModel(item)).ToList();
                return JsonSuccess(
                    new PaginatedResponseModel<LogResponseModel>(responseList, totalItems)
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