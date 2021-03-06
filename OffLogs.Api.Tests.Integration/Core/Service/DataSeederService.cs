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
        
        public DataSeederService(
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
    }
}