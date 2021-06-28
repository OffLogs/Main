using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public partial class DataSeederService: IDataSeederService
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
            IApplicationDao applicationDao, 
            ILogDao logDao
        )
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
            var fakeApplication = _factory.ApplicationFactory(user).Generate();
            var application = await _applicationDao.CreateNewApplication(user, fakeApplication.Name);
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
                application.Id = (long)await _applicationDao.InsertAsync(application);
            }
            return result;
        }
    }
}