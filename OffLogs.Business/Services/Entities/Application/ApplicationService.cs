using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.Application
{
    public class ApplicationService: IApplicationService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtApplicationService _jwtService;

        public ApplicationService(
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder asyncQueryBuilder,
            IJwtApplicationService jwtService
        )
        {
            _commandBuilder = commandBuilder;
            this._queryBuilder = asyncQueryBuilder;
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

        public async Task<bool> IsOwner(long userId, long applicationId)
        {
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(applicationId);
            return await IsOwner(userId, application);
        }

        public async Task<bool> IsOwner(long userId, ApplicationEntity application)
        {
            return await Task.FromResult(application.User.Id == userId);
        }
    }
}