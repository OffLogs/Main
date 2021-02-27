using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Models.Response;
using OffLogs.Business.Mvc.Controller;

namespace OffLogs.Api.Controller
{
    [ApiController]
    public class HomeController : BaseApiController
    {
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration) : base(logger, configuration)
        {
        }
        
        [HttpGet("/ping")]
        public IActionResult Ping()
        {
            return JsonSuccess(new PongResponseModel());
        }
    }
}