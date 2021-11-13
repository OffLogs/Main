using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
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
            // var existsUser = await _queryBuilder.For<UserEntity>()
            //     .WithAsync(new UserGetByCriteria(request.UserName));
            //
            //
            // if (existsUser == null)
            // {
            //     throw new UserNotAuthorizedException();
            // }
            // var passwordHash = SecurityUtil.GeneratePasswordHash(
            //     request.Password, 
            //     existsUser.PasswordSalt
            // );
            // if (!passwordHash.CompareTo(existsUser.PublicKey))
            // {
            //     throw new UserNotAuthorizedException();
            // }
            return new LoginResponseDto()
            {
                // Token = _jwtAuthService.BuildJwt(existsUser.Id)
                Token = ""
            };
        }
    }
}
