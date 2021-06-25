using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OffLogs.Business.Constants;
using OffLogs.Business.Services;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Business.Mvc.Attribute.Auth
{
    [System.AttributeUsage(AttributeTargets.Method)]
    public class OnlyAuthorizedApplicationAttribute : System.Attribute, IActionFilter
    {   
        public void OnActionExecuted(ActionExecutedContext context) { }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            var myAuthorizationService = context.HttpContext.RequestServices.GetService(typeof(IJwtApplicationService)) as IJwtApplicationService;
            if (myAuthorizationService == null)
            {
                throw new ArgumentNullException("_MyAuthorizationService service not found");
            }

            if (!myAuthorizationService.IsValidJwt())
            {
                context.Result = new ObjectResult(new {
                    Status = HttpResponseStatus.Fail.GetValue()
                }) 
                { 
                    StatusCode = (int)HttpStatusCode.Unauthorized 
                };
            }
        }
    }
}