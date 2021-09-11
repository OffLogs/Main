using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Mvc.Filters
{
    public class AsyncExceptionFilter: IAsyncExceptionFilter
    {
        private readonly ILogger<AsyncExceptionFilter> _logger;

        public AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = new JsonResult(
                    new BadRequestObjectResult(new
                    {
                        Type = "Fail",
                        Message = "Server error"
                    })   
            );
            return Task.CompletedTask;
        }
    }
}