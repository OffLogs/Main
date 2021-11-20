using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Common.Extensions;
using OffLogs.Business.Common.Security;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Exceptions;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class RegistrationStep2RequestHandler : IAsyncRequestHandler<RegistrationStep2Request, RegistrationStep2ResponseDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IUserService _userService;

        public RegistrationStep2RequestHandler(
            IAsyncQueryBuilder queryBuilder, 
            IJwtAuthService jwtAuthService,
            IUserService userService
        )
        {
            _queryBuilder = queryBuilder;
            _jwtAuthService = jwtAuthService;
            _userService = userService;
        }

        public async Task<RegistrationStep2ResponseDto> ExecuteAsync(RegistrationStep2Request request)
        {
            var user = await _queryBuilder.For<UserEntity>()
                .WithAsync(new UserGetPendingCriteria(request.Token));
            if (user == null)
            {
                throw new EntityIsNotExistException();
            }

            var (_, pemFile) = await _userService.ActivateUser(user.Id, request.Password);
            return new RegistrationStep2ResponseDto()
            {
                JwtToken = _jwtAuthService.BuildJwt(user.Id),
                Pem = pemFile,
            };
        }
    }
}