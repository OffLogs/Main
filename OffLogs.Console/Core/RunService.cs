using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Console.Core
{
    public class RunService: IRunService
    {
        private readonly IDataFactoryService _factory;
        private readonly IUserDao _userDao;
        private readonly IApplicationDao _applicationDao;
        private readonly IJwtAuthService _jwtAuthService;
        
        public RunService(
            IDataFactoryService factoryService, 
            IUserDao userDao, 
            IJwtAuthService jwtAuthService, 
            IApplicationDao applicationDao
        )
        {
            _factory = factoryService;
            _userDao = userDao;
            _jwtAuthService = jwtAuthService;
            _applicationDao = applicationDao;
        }
        
        public void RunStart()
        {
            System.Console.WriteLine("User is created: ");
        }
    }
}