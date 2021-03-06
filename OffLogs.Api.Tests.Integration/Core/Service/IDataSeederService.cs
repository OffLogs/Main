using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public interface IDataSeederService
    {
        Task<UserTestModel> CreateNewUser();
    }
}