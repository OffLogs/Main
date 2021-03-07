using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace OffLogs.Business.Extensions
{
    public static class HttpRequestExtension
    {
        private const string ApiTokenKey = "api_token";
        
        public static string GetApiToken(this HttpRequest request)
        {
            string authToken = null;
            if (request.Query.ContainsKey(ApiTokenKey))
            {
                authToken = request.Query[ApiTokenKey];
            }
            if (string.IsNullOrEmpty(authToken) && request.Headers.ContainsKey("Authorization"))
            {
                authToken = request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authToken) && authToken.StartsWith("Bearer "))
                {
                    authToken = authToken.Substring(7);
                }
            }
            return authToken;
        }
    }
}