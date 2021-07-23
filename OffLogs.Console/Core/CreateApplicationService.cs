using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries.Entities.User;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Entities.Application;
using OffLogs.Business.Services.Entities.User;
using OffLogs.Console.Verbs;
using Persistence.Transactions.Behaviors;
using Queries.Abstractions;

namespace OffLogs.Console.Core
{
    public class CreateApplicationService: ICreateApplicationService
    {
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IApplicationService _applicationService;
        private readonly ILogger<CreateApplicationService> _logger;
        private readonly IDbSessionProvider _dbSessionProvider;

        public CreateApplicationService(
            IAsyncQueryBuilder queryBuilder,
            IApplicationService applicationService,
            ILogger<CreateApplicationService> logger,
            IDbSessionProvider dbSessionProvider
        )
        {
            _queryBuilder = queryBuilder;
            _applicationService = applicationService;
            _logger = logger;
            _dbSessionProvider = dbSessionProvider;
        }

        public async Task<int> Create(CreateNewApplicationVerb verb)
        {
            try
            {
                _logger.LogInformation("Create application starting..");
                var user = await _queryBuilder.For<UserEntity>()
                    .WithAsync(new UserGetByCriteria(verb.UserName));
                if (user == null)
                {
                    _logger.LogError("User not found!");    
                    return 1;
                }
                _logger.LogInformation("User was found...");  
                var application = await _applicationService.CreateNewApplication(user, verb.Name);
                
                _logger.LogInformation("Application is created!");
                _logger.LogInformation("----------------------------------");
                _logger.LogInformation($"UserName: {user.UserName:S}");
                _logger.LogInformation($"Application Name: {application.Name:S}");
                _logger.LogInformation($"Application API Token: {application.ApiToken:S}");
                _logger.LogInformation("----------------------------------");

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