using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using OffLogs.Console.Verbs;

namespace OffLogs.Console.Core
{
    public class CreateApplicationService: ICreateApplicationService
    {
        private readonly IDataFactoryService _factory;
        private readonly IUserDao _userDao;
        private readonly IApplicationDao _applicationDao;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly ILogger<CreateApplicationService> _logger;
        
        public CreateApplicationService(
            IDataFactoryService factoryService, 
            IUserDao userDao, 
            IJwtAuthService jwtAuthService, 
            IApplicationDao applicationDao, 
            ILogger<CreateApplicationService> logger
        )
        {
            _factory = factoryService;
            _userDao = userDao;
            _jwtAuthService = jwtAuthService;
            _applicationDao = applicationDao;
            _logger = logger;
        }

        public async Task<int> Create(CreateNewApplicationVerb verb)
        {
            try
            {
                _logger.LogInformation("Create application starting..");
                var user = await _userDao.GetByUserName(verb.UserName);
                if (user == null)
                {
                    _logger.LogError("User not found!");    
                    return 1;
                }
                _logger.LogInformation("User was found...");  
                var application = await _applicationDao.CreateNewApplication(user.Id, verb.Name);
                
                _logger.LogInformation("Application is created!");
                _logger.LogInformation("----------------------------------");
                _logger.LogInformation($"UserName: {user.UserName:S}");
                _logger.LogInformation($"Application Name: {application.Name:S}");
                _logger.LogInformation($"Application API Token: {application.ApiToken:S}");
                _logger.LogInformation("----------------------------------");
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