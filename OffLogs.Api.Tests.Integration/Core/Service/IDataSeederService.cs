using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Tests.Integration.Core.Models;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public interface IDataSeederService
    {
        Task<UserTestModel> CreateNewUser();
        Task<List<LogEntity>> CreateLogs(long applicationId, LogLevel level, int counter = 1);
    }
}