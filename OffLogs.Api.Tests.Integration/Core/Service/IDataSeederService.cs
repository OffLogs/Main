using System.Threading.Tasks;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public interface IDataSeederService
    {
        Task<UserEntity> CreateNewUser();
    }
}