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
    public class JwtApplicationService: IJwtApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _key;
        
        private string JwtToken => _httpContext.HttpContext?.Request.GetApiToken();

        public JwtApplicationService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetValue<string>("App:Application:SymmetricSecurityKey")
                )
            );
            _issuer = _configuration.GetValue<string>("App:Application:Issuer");
            _audience = _configuration.GetValue<string>("App:Application:Audience");
        }

        public string GetToken()
        {
            return JwtToken;
        }

        public string BuildJwt(long applicationId)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.System, "Application"),
                new(ClaimTypes.NameIdentifier, applicationId.ToString())
            };
            
            var signingCredentials =
                new SigningCredentials(
                    _key, 
                    SecurityAlgorithms.HmacSha256
                );
            var jwt = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                signingCredentials: signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        public long? GetApplicationId(string jwtString = null)
        {
            try
            {   
                var jwt = new JwtSecurityToken(string.IsNullOrEmpty(jwtString) ? JwtToken : jwtString);
                var applicationId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return applicationId != null ? int.Parse(applicationId) : null;
            }
            catch (Exception)
            {
                return null;
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
                ValidateLifetime = false,
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
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}