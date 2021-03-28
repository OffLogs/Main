using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public class DataSeederService: IDataSeederService
    {
        private readonly IDataFactoryService _factory;
        private readonly IUserDao _userDao;
        private readonly IApplicationDao _applicationDao;
        private readonly IJwtAuthService _jwtAuthService;
        private readonly ILogDao _logDao;
        
        public DataSeederService(
            IDataFactoryService factoryService, 
            IUserDao userDao, 
            IJwtAuthService jwtAuthService, 
            IApplicationDao applicationDao, ILogDao logDao)
        {
            _factory = factoryService;
            _userDao = userDao;
            _jwtAuthService = jwtAuthService;
            _applicationDao = applicationDao;
            _logDao = logDao;
        }

        public async Task<UserTestModel> CreateNewUser()
        {
            var fakeUser = _factory.UserFactory().Generate();
            var user = new UserTestModel(
                await _userDao.CreateNewUser(fakeUser.UserName, fakeUser.Email)    
            );
            var fakeApplication = _factory.ApplicationFactory(user.Id).Generate();
            var application = await _applicationDao.CreateNewApplication(fakeApplication.UserId, fakeApplication.Name);
            user.Applications.Add(
                application
            );
            user.ApiToken = _jwtAuthService.BuildJwt(user.Id);
            return user;
        }
        
        public Task<List<LogEntity>> CreateLogs(long applicationId, LogLevel level, int counter = 1)
        {
            var logFactory = _factory.LogFactory(applicationId, level);
            var result = new List<LogEntity>();
            for (int i = 1; i <= counter; i++)
            {
                var log = logFactory.Generate();
                _logDao.GetConnection().Insert(log);
                result.Add(log);
                
                var logTraceFactory = _factory.LogTraceFactory(log.Id);
                logTraceFactory.GenerateLazy(4)
                    .ToList()
                    .ForEach(item =>
                    {
                        _logDao.GetConnection().Insert(item);
                    });
                var logPropertyFactory = _factory.LogPropertyFactory(log.Id);
                logPropertyFactory.GenerateLazy(3)
                    .ToList()
                    .ForEach(item =>
                    {
                        _logDao.GetConnection().Insert(item);
                    });
            }

            return Task.FromResult(result);
        }
    }
}