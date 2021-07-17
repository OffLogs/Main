using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Constants;
using OffLogs.Business.Dao;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public partial class DataSeederService: IDataSeederService
    {
        private readonly IDataFactoryService _factory;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        public DataSeederService(
            IDataFactoryService factoryService, 
            IAsyncCommandBuilder commandBuilder, 
            IJwtAuthService jwtAuthService,
            IUserService userService,
            IApplicationService applicationService,
            IAsyncQueryBuilder queryBuilder
        )
        {
            _factory = factoryService;
            _commandBuilder = commandBuilder;
            _jwtAuthService = jwtAuthService;
            _userService = userService;
            _applicationService = applicationService;
            _queryBuilder = queryBuilder;
        }

        public async Task<UserTestModel> CreateNewUser()
        {
            var fakeUser = _factory.UserFactory().Generate();
            var user = new UserTestModel(
                await _userService.CreateNewUser(fakeUser.UserName, fakeUser.Email)    
            );
            var fakeApplication = _factory.ApplicationFactory(user).Generate();
            var application = await _applicationService.CreateNewApplication(user, fakeApplication.Name);
            user.Applications.Add(
                application
            );
            user.ApiToken = _jwtAuthService.BuildJwt(user.Id);
            return user;
        }

        public async Task<List<ApplicationEntity>> CreateApplicationsAsync(UserEntity user, int counter = 1)
        {
            var factory = _factory.ApplicationFactory(user);
            var result = new List<ApplicationEntity>();
            for (int i = 1; i <= counter; i++)
            {
                var application = factory.Generate();
                result.Add(application);
                await _commandBuilder.SaveAsync(application);
            }
            return result;
        }
    }
}