using System.Collections.Generic;
using System.Threading.Tasks;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Entities.User;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public interface IDataSeederService
    {
        Task<UserTestModel> CreateActivatedUser(string email = null);
        Task<List<LogEntity>> CreateLogsAsync(long applicationId, LogLevel level, int counter = 1);
        Task<LogEntity> MakeLogAsync(ApplicationEntity application, LogLevel level);
        Task<List<ApplicationEntity>> CreateApplicationsAsync(UserEntity user, int counter = 1);
    }
}
