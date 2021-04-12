using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Models.Http;
using OffLogs.Business.Constants;

namespace OffLogs.Business.Mvc.Controller
{
    public class BaseApiController: Microsoft.AspNetCore.Mvc.Controller
    {
        protected readonly ILogger<BaseApiController> logger;

        protected readonly string DefaultSiteUrl;
        
        public BaseApiController(
            ILogger<BaseApiController> logger,
            IConfiguration configuration
        )
        {
            this.logger = logger;
            DefaultSiteUrl = configuration.GetValue<string>("App:DefaultSiteUrl");
        }
        
        protected JsonResult JsonSuccess(object data = null, string message = null)
        {
            return new(
                new JsonCommonResponse()
                {
                    Status = HttpResponseStatus.Ok.GetValue(),
                    Message = message,
                    Data = data ?? new {}
                }
            );
        }

        protected JsonResult JsonError(HttpStatusCode statusCode)
        {
            return JsonError(null, statusCode);
        }

        protected JsonResult JsonError(string message = null, HttpStatusCode? statusCode = null)
        {
            var response = new JsonResult(
                new JsonCommonResponse()
                {
                    Status = HttpResponseStatus.Fail.GetValue(),
                    Message = message
                }
            );
            response.StatusCode = (int)(statusCode ?? HttpStatusCode.BadRequest);
            return response;
        }
    }
}
