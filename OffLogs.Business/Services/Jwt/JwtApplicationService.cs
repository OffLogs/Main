using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Jwt
{
    public class JwtApplicationService: IJwtApplicationService
    {
        private readonly IConfiguration _configuration;
        
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _key;
        
        public JwtApplicationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetValue<string>("App:Application:SymmetricSecurityKey")
                )
            );
            _issuer = _configuration.GetValue<string>("App:Application:Issuer");
            _audience = _configuration.GetValue<string>("App:Application:Audience");
        }

        public string BuildJwt(ApplicationEntity application)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.System, "Application"),
                new(ClaimTypes.NameIdentifier, application.Id.ToString()),
                new(ClaimTypes.Sid, application.User.Id.ToString()),
                new(ClaimTypes.Rsa, Convert.ToBase64String(application.PublicKey)),
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
        
        public long? GetApplicationId(string jwtString)
        {
            jwtString = jwtString ?? throw new ArgumentNullException(nameof(jwtString));
            try
            {   
                var jwt = new JwtSecurityToken(jwtString);
                var applicationId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return applicationId != null ? int.Parse(applicationId) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public long? GetUserId(string jwtString)
        {
            jwtString = jwtString ?? throw new ArgumentNullException(nameof(jwtString));
            try
            {   
                var jwt = new JwtSecurityToken(jwtString);
                var applicationId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                return applicationId != null ? int.Parse(applicationId) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public byte[]? GetApplicationPublicKey(string jwtString)
        {
            jwtString = jwtString ?? throw new ArgumentNullException(nameof(jwtString));
            try
            {   
                var jwt = new JwtSecurityToken(jwtString);
                var publicKeyBase64 = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Rsa)?.Value;
                return publicKeyBase64 != null ? Convert.FromBase64String(publicKeyBase64) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool IsValidJwt(string token)
        {
            token = token ?? throw new ArgumentNullException(nameof(token));
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
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
