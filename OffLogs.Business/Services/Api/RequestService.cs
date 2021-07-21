using Microsoft.AspNetCore.Http;
using OffLogs.Business.Extensions;
using OffLogs.Business.Services.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Api
{
    public class RequestService: IRequestService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJwtAuthService _jwtAuthService;

        public RequestService(
            IHttpContextAccessor httpContext,
            IJwtAuthService jwtAuthService
        )
        {
            _httpContext = httpContext;
            _jwtAuthService = jwtAuthService;
        }

        public string GetApiToken()
        {
            return _httpContext.HttpContext?.Request.GetApiToken();
        }

        public long GetUserIdFromJwt()
        {
            return _jwtAuthService.GetUserId(GetApiToken());
        }
    }
}
