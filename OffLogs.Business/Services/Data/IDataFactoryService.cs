using Bogus;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Business.Services.Data
{
    public interface IDataFactoryService
    {
        Faker<UserEntity> UserFactory();
        Faker<ApplicationEntity> ApplicationFactory(int userId);
    }
}