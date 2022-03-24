using System;
using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Common.Security;
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
            var asymmetricEncryptor = AsymmetricEncryptor.ReadFromPem(request.Pem, request.Password);
            
            var publicKey = asymmetricEncryptor.GetPublicKeyBytes();
            var existsUser = await _queryBuilder.For<UserEntity>()
                .WithAsync(new UserGetByCriteria(publicKey));
             
            if (existsUser == null)
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
