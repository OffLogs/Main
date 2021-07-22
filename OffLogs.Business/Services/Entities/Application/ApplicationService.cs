using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Abstractions;
using Domain.Abstractions;
using OffLogs.Business.Exceptions;
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

        public async Task<ApplicationEntity> UpdateApplication(long applicationId, string name)
        {
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(applicationId);
            if (application == null)
            {
                throw new ItemNotFoundException(nameof(ApplicationEntity));
            }
            application.Name = name;
            application.UpdateTime = System.DateTime.UtcNow;
            await _commandBuilder.SaveAsync(application);
            return application;
        }

        /// <summary>
        /// Should add rights for the other user for this app
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ShareForUser(ApplicationEntity application, UserEntity user)
        {
            if (application.User.Id == user.Id)
            {
                throw new PermissionException("This is application owner");
            }
            if (application.SharedForUsers.Any(u => u.Id == user.Id))
            {
                throw new PermissionException("This user already has access right for this application");
            }
            application.SharedForUsers.Add(user);
            await _commandBuilder.SaveAsync(application);
        }
    }
}