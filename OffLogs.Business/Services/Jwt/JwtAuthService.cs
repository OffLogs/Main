using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OffLogs.Business.Extensions;

namespace OffLogs.Business.Services.Jwt
{
    public class JwtAuthService: IJwtAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _key;
        
        private string JwtToken => _httpContext.HttpContext?.Request.GetApiToken();
        
        public JwtAuthService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetValue<string>("App:Auth:SymmetricSecurityKey")
                )
            );
            _issuer = _configuration.GetValue<string>("App:Auth:Issuer");
            _audience = _configuration.GetValue<string>("App:Auth:Audience");
        }

        public string GetToken()
        {
            return JwtToken;
        }

        public string BuildJwt(long userId)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, "user"),
                new(ClaimsIdentity.DefaultRoleClaimType, "user"),
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };
            
            var jwtSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetValue<string>("App:Auth:SymmetricSecurityKey")
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
                claims: claims,
                expires: expirationTime,
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        public long GetUserId(string jwtString = null)
        {
            try
            {   
                var jwt = new JwtSecurityToken(string.IsNullOrEmpty(jwtString) ? JwtToken : jwtString);
                return long.Parse(jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        
        public bool IsValidJwt(string jwtString = null)
        {
            var token = string.IsNullOrEmpty(jwtString) ? JwtToken : jwtString;
            var parameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = _key
            };
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(
                    token,
                    parameters,
                    out SecurityToken validatedToken
                );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}