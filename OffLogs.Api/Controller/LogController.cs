using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request.Log.Common;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Mvc.Attribute.Auth;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Controller
{
    [Route("/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        private readonly IJwtApplicationService _jwtService;
        private readonly ILogDao _logDao;
        
        public LogController(
            ILogger<LogController> logger, 
            IConfiguration configuration,
            ILogDao logDao,
            IJwtApplicationService jwtService
        ) : base(logger, configuration)
        {
            _logDao = logDao;
            _jwtService = jwtService;
        }
        
        [OnlyAuthorizedApplication]
        [HttpPost("add")]
        public async Task<IActionResult> LogAction([FromBody]AddCommonLogsRequestModel model)
        {
            if (!model.Logs.Any())
            {
                return JsonSuccess();
            }
            try
            {
                var applicationId = _jwtService.GetApplicationId().Value;
                foreach (var log in model.Logs)
                {
                    var properties = log.Properties.Select(
                        property => new LogPropertyEntity(property.Key, property.Value)
                    ).ToArray();
                    var traces = log.Traces?.Select(
                        trace => new LogTraceEntity(trace)
                    ).ToArray();
                    await _logDao.AddAsync(
                        applicationId,
                        log.Message,
                        log.LogLevel,
                        log.Timestamp,
                        properties,
                        traces
                    );    
                }
                return JsonSuccess();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return JsonError();
            }
        }
        
        [OnlyAuthorizedApplication]
        [HttpPost("add/serilog")]
        public async Task<IActionResult> LogSerilogAction([FromBody]AddSerilogLogsRequestModel model)
        {
            if (!model.Events.Any())
            {
                return JsonSuccess();
            }
            try
            {
                var applicationId = _jwtService.GetApplicationId().Value;
                foreach (var log in model.Events)
                {
                    var properties = log.Properties.Select(
                        property => new LogPropertyEntity(property.Key, property.Value)
                    ).ToArray();
                    var traces = log.Exception?.Split("\n").Select(
                        trace => new LogTraceEntity(trace)
                    ).ToArray();
                    await _logDao.AddAsync(
                        applicationId,
                        log.RenderedMessage,
                        log.LogLevel.ToLogLevel(),
                        log.Timestamp,
                        properties,
                        traces
                    );    
                }
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