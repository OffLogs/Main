using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class RegistrationStep1RequestHandler : IAsyncRequestHandler<RegistrationStep1Request, RegistrationStep1ResponseDto>
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtAuthService _jwtAuthService;

        public RegistrationStep1RequestHandler(
            IAsyncQueryBuilder queryBuilder, 
            IJwtAuthService jwtAuthService
        )
        {
            _queryBuilder = queryBuilder;
            _jwtAuthService = jwtAuthService;
        }

        public async Task<RegistrationStep1ResponseDto> ExecuteAsync(RegistrationStep1Request request)
        {
            return new RegistrationStep1ResponseDto()
            {
                
            };
        }
    }
}