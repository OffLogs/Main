using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Api.Services.LogParser;
using OffLogs.Business.Mvc.Attribute.Auth;
using OffLogs.Business.Mvc.Controller;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Controller
{
    [Route("/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        private readonly ISerilogLogParserService _serilogLogParserService;
        private readonly IJwtApplicationService _jwtService;
        
        public LogController(
            ILogger<LogController> logger, 
            IConfiguration configuration,
            ISerilogLogParserService serilogLogParserService,
            IJwtApplicationService jwtService
        ) : base(logger, configuration)
        {
            _serilogLogParserService = serilogLogParserService;
            _jwtService = jwtService;
        }
        
        [OnlyAuthorizedApplication]
        [HttpPost("serilog")]
        public async Task<IActionResult> LogSerilog([FromBody]SerilogEventsRequestModel model)
        {
            if (!model.Events.Any())
            {
                return JsonSuccess();
            }
            await _serilogLogParserService.SaveAsync(_jwtService.GetApplicationId().Value, model);
            return JsonSuccess();
        }
    }
}