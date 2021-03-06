using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;

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

        public int CreateUser()
        {
            _logger.LogInformation("Create user starting..");
            return 0;
        }
    }
}