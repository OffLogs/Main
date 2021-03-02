using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Api.Services.LogParser;
using OffLogs.Business.Mvc.Controller;

namespace OffLogs.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        private readonly ISerilogLogParserService _serilogLogParserService;
        
        public LogController(
            ILogger<LogController> logger, 
            IConfiguration configuration,
            ISerilogLogParserService serilogLogParserService
        ) : base(logger, configuration)
        {
            _serilogLogParserService = serilogLogParserService;
        }
        
        [HttpPost("serilog")]
        public async Task<IActionResult> LogSerilog(SerilogEventsRequestModel model)
        {
            if (!model.Events.Any())
            {
                return JsonSuccess();
            }
            await _serilogLogParserService.SaveAsync(model);
            return JsonSuccess();
        }
    }
}