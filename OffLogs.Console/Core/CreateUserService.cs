using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Utils;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Console.Verbs;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Console.Core
{
    public class CreateUserService: ICreateUserService
    {
        private readonly ILogger<CreateUserService> _logger;
        private readonly IUserService _userService;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly IDbSessionProvider _dbSessionProvider;

        public CreateUserService(
            ILogger<CreateUserService> logger,
            IUserService userService,
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            IDbSessionProvider dbSessionProvider
        )
        {
            _logger = logger;
            _userService = userService;
            _queryBuilder = queryBuilder;
            _applicationService = applicationService;
            _dbSessionProvider = dbSessionProvider;
        }

        public async Task<int> CreateUser(CreateNewUserVerb verb)
        {
            try
            {
                _logger.LogInformation("Create user starting..");
                var password = SecurityUtil.GeneratePassword(8);
                var pendingUser = await _userService.CreatePendingUser(verb.UserName, verb.Email);
                var (user, pemFile) = await _userService.ActivateUser(
                    pendingUser.Id,
                    password
                );
                _logger.LogInformation("User is created!");
                _logger.LogInformation($"----------------------------------");
                _logger.LogInformation($"UserName: {user.UserName}");
                _logger.LogInformation($"Pem file password: {password}");
                _logger.LogInformation($"Pem file: \n{pemFile}\n");
                _logger.LogInformation($"----------------------------------");

                await _dbSessionProvider.PerformCommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return 1;
            }
            return 0;
        }
    }
}