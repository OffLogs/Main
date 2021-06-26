using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Mvc.Controller;

namespace OffLogs.Api.Frontend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        public LogController(ILogger<BaseApiController> logger, IConfiguration configuration) : base(logger, configuration)
        {
            
        }
    }
}