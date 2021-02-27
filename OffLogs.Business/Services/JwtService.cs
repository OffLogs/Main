using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace OffLogs.Business.Services
{
    public class JwtService: IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        
        private string JwtToken
        {
            get {
                if (_httpContext
                    .HttpContext != null)
                {
                    string authString = _httpContext
                        .HttpContext
                        .Request
                        .Headers["authorization"]
                        .ToString();
                    return authString.Substring(7, authString.Length - 7);
                }
                return null;
            }
        }
        
        public JwtService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public string GetToken()
        {
            return JwtToken;
        }

        public string BuildJwt(int userId)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, "user"),
                new(ClaimsIdentity.DefaultRoleClaimType, "user"),
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, 
                "Token", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            var jwtSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetValue<string>("Auth:SymmetricSecurityKey")
                )
            );
            var now = DateTime.UtcNow;
            var expirationTime = now.Add(TimeSpan.FromMinutes(
                _configuration.GetValue<int>("App:Auth:Lifetime")
            ));
            var signingCredentials =
                new SigningCredentials(
                    jwtSecurityKey, 
                    SecurityAlgorithms.HmacSha256
                );
            var jwt = new JwtSecurityToken(
                _configuration.GetValue<string>("App:Auth:Issuer"),
                _configuration.GetValue<string>("App:Auth:Audience"),
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: expirationTime,
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public int? GetUserId(string jwtString = null)
        {
            try
            {
                var jwt = new JwtSecurityToken(string.IsNullOrEmpty(jwtString) ? JwtToken : jwtString);
                return int.Parse(jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}