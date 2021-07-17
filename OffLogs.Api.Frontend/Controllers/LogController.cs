using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Frontend.Models.Request.Log.Common;
using OffLogs.Business.Mvc.Attribute.Auth;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Kafka;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OffLogs.Api.Frontend.Models.Request.Log.Serilog;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka.Models;

namespace OffLogs.Api.Frontend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IJwtApplicationService _jwtApplicationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogController(
            ILogger<BaseApiController> logger, 
            IConfiguration configuration,
            IKafkaProducerService kafkaProducerService,
            IJwtApplicationService jwtApplicationService,
            IHttpContextAccessor httpContextAccessor
        ) : base(logger, configuration)
        {
            _kafkaProducerService = kafkaProducerService;
            _jwtApplicationService = jwtApplicationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [OnlyAuthorizedApplication]
        [HttpPost("add")]
        public async Task<IActionResult> LogAction([FromBody] AddCommonLogsRequestModel model)
        {
            if (!model.Logs.Any())
            {
                return JsonSuccess();
            }
            try
            {
                var token = _jwtApplicationService.GetToken();
                var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
                
                foreach (var log in model.Logs)
                {
                    var logEntity = new LogEntity()
                    {
                        Message = log.Message,
                        Level = log.LogLevel,
                        LogTime = log.Timestamp
                    };
                    if (log.Properties != null)
                    {
                        foreach (var property in log.Properties)
                        {
                            logEntity.AddProperty(new LogPropertyEntity(property.Key, property.Value));
                        }
                    }
                    if (log.Traces != null)
                    {
                        foreach (var trace in log.Traces)
                        {
                            logEntity.AddTrace(new LogTraceEntity(trace));
                        }
                    }
                    await _kafkaProducerService.ProduceLogMessageAsync(token, logEntity, clientIp);
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
                var token = _jwtApplicationService.GetToken();
                var clientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
                foreach (var log in model.Events)
                {
                    var logEntity = new LogEntity()
                    {
                        Message = log.RenderedMessage,
                        Level = log.LogLevel.ToLogLevel(),
                        LogTime = log.Timestamp
                    };
                    if (log.Properties != null)
                    {
                        foreach (var property in log.Properties)
                        {
                            logEntity.AddProperty(new LogPropertyEntity(property.Key, property.Value));
                        }
                    }

                    var traces = log.Exception?.Split("\n");
                    if (traces != null)
                    {
                        foreach (var trace in traces)
                        {
                            logEntity.AddTrace(new LogTraceEntity(trace));
                        }
                    }
                    await _kafkaProducerService.ProduceLogMessageAsync(token, logEntity, clientIp); 
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