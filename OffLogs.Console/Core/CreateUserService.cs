using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using OffLogs.Console.Verbs;

namespace OffLogs.Console.Core
{
    public class CreateUserService: ICreateUserService
    {
        private readonly IDataFactoryService _factory;
        private readonly IUserDao _userDao;
        private readonly IApplicationDao _applicationDao;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly ILogger<CreateUserService> _logger;
        
        public CreateUserService(
            IDataFactoryService factoryService, 
            IUserDao userDao, 
            IJwtAuthService jwtAuthService, 
            IApplicationDao applicationDao, 
            ILogger<CreateUserService> logger
        )
        {
            _factory = factoryService;
            _userDao = userDao;
            _jwtAuthService = jwtAuthService;
            _applicationDao = applicationDao;
            _logger = logger;
        }

        public async Task<int> CreateUser(CreateNewUserVerb verb)
        {
            try
            {
                _logger.LogInformation("Create user starting..");
                var user = await _userDao.CreateNewUser(verb.UserName, verb.Email);
                _logger.LogInformation("User is created!");
                _logger.LogInformation($"----------------------------------");
                _logger.LogInformation($"UserName: {user.UserName}");
                _logger.LogInformation($"Password: {user.Password}");
                _logger.LogInformation($"----------------------------------");
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