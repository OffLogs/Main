using Microsoft.AspNetCore.Mvc;

namespace OffLogs.Api.Controller
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Json(new { Pong = true });
        }
    }
}