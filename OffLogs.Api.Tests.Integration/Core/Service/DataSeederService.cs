using System.Threading.Tasks;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Data;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public class DataSeederService: IDataSeederService
    {
        private readonly IDataFactoryService _factory;
        private readonly IUserDao _userDao;
        private readonly IApplicationDao _applicationDao;
        
        public DataSeederService(IDataFactoryService factoryService, IUserDao userDao, IApplicationDao applicationDao)
        {
            _factory = factoryService;
            _userDao = userDao;
            _applicationDao = applicationDao;
        }

        public async Task<UserEntity> CreateNewUser()
        {
            var fakeUser = _factory.UserFactory().Generate();
            var user = await _userDao.CreateNewUser(fakeUser.UserName, fakeUser.Email);
            var fakeApplication = _factory.ApplicationFactory(user.Id).Generate();
            var application = await _applicationDao.CreateNewApplication(fakeApplication.UserId, fakeApplication.Name);
            user.Applications.Add(
                application
            );
            return user;
        }
    }
}