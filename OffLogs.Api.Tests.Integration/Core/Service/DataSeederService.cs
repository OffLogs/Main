using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.Log;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public partial class DataSeederService: IDataSeederService
    {
        private readonly IDataFactoryService _dataFactory;
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly ILogService _logService;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;

        public DataSeederService(
            IDataFactoryService dataFactoryService, 
            IAsyncCommandBuilder commandBuilder, 
            IJwtAuthService jwtAuthService,
            IUserService userService,
            IApplicationService applicationService,
            IAsyncQueryBuilder queryBuilder,
            ILogService logService
        )
        {
            _dataFactory = dataFactoryService;
            _commandBuilder = commandBuilder;
            _jwtAuthService = jwtAuthService;
            _userService = userService;
            _applicationService = applicationService;
            _queryBuilder = queryBuilder;
            _logService = logService;
        }

        public async Task<UserTestModel> CreateActivatedUser(string userName = null, string email = null)
        {
            var fakeUser = _dataFactory.UserFactory().Generate();
            fakeUser.UserName ??= userName;
            fakeUser.Email ??= email;
            var user = await _userService.CreatePendingUser(fakeUser.Email);
            var pemFilePassword = SecurityUtil.GeneratePassword();
            var (activatedUser, pemFile) = await _userService.ActivateUser(
                user.Id, 
                pemFilePassword
            );
            var userModel = new UserTestModel(activatedUser, pemFilePassword, pemFile);
            var fakeApplication = _dataFactory.ApplicationFactory(userModel).Generate();
            var application = await _applicationService.CreateNewApplication(userModel, fakeApplication.Name);
            userModel.Applications.Add(
                application
            );
            userModel.ApiToken = _jwtAuthService.BuildJwt(userModel.Id);
            return userModel;
        }

        public async Task<List<ApplicationEntity>> CreateApplicationsAsync(UserEntity user, int counter = 1)
        {
            var factory = _dataFactory.ApplicationFactory(user);
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
