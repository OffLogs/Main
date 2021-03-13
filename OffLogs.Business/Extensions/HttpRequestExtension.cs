using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        
        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            var result = "";
            try
            {
                request.EnableBuffering();
                // Arguments: Stream, Encoding, detect encoding, buffer size 
                // AND, the most important: keep stream opened
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    result = await reader.ReadToEndAsync();
                }
            }
            finally
            {
                // Rewind, so the core is not lost when it looks the body for the request
                request.Body.Position = 0;
            }
            return result;
        }
    }
}