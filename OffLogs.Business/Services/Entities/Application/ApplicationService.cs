using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Business.Services.Entities.Application
{
    public class ApplicationService: IApplicationService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IJwtApplicationService _jwtService;

        public ApplicationService(
            IAsyncCommandBuilder commandBuilder, 
            IJwtApplicationService jwtService
        )
        {
            _commandBuilder = commandBuilder;
            _jwtService = jwtService;
        }

        public async Task<ApplicationEntity> CreateNewApplication(UserEntity user,  string name)
        {   
            var application = new ApplicationEntity(user, name);
            await _commandBuilder.SaveAsync(application);
            application.ApiToken = _jwtService.BuildJwt(application.Id);
            await _commandBuilder.SaveAsync(application);
            return application;
        }
    }
}