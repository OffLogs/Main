using System.Threading.Tasks;
using Api.Requests.Abstractions;
using Microsoft.AspNetCore.Http;
using OffLogs.Api.Controller.Public.User.Dto;
using OffLogs.Api.Exceptions;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class LoginRequestHandler : IAsyncRequestHandler<LoginRequest, LoginResponseDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginRequestHandler(
            IAsyncQueryBuilder queryBuilder, 
            IJwtAuthService jwtAuthService
        )
        {
            _queryBuilder = queryBuilder;
            _jwtAuthService = jwtAuthService;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequest request)
        {
            var existsUser = await _queryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(request.UserName));
            
        
            if (existsUser == null)
            {
                throw new UserNotAuthorizedException();
            }
            var passwordHash = SecurityUtil.GeneratePasswordHash(
                request.Password, 
                existsUser.PasswordSalt
            );
            if (!passwordHash.CompareTo(existsUser.PasswordHash))
            {
                throw new UserNotAuthorizedException();
            }
            return new LoginResponseDto()
            {
                Token = _jwtAuthService.BuildJwt(existsUser.Id)
            };
        }
    }
}