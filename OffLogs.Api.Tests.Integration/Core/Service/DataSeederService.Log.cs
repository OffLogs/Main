using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entities;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public partial class DataSeederService
    {

        public async Task<LogEntity> MakeLogAsync(ApplicationEntity application, LogLevel level)
        {
            var logFactory = _factory.LogFactory(level);
            var log = logFactory.Generate();
            log.Application = application;
                
            var logTraceFactory = _factory.LogTraceFactory();
            logTraceFactory.GenerateLazy(4)
                .ToList()
                .ForEach(item =>
                {   
                    log.Traces.Add(item);
                });
            var logPropertyFactory = _factory.LogPropertyFactory();
            logPropertyFactory.GenerateLazy(3)
                .ToList()
                .ForEach(item =>
                {
                    log.Properties.Add(item);
                });

            return await Task.FromResult(log);
        }
        
        public async Task<List<LogEntity>> CreateLogsAsync(ApplicationEntity application, LogLevel level, int counter = 1)
        {
            var result = new List<LogEntity>();
            for (int i = 1; i <= counter; i++)
            {
                var log = await MakeLogAsync(application, level);
                await _logDao.AddAsync(log);
                result.Add(log);
            }
            return result;
        }

        public async Task<List<LogEntity>> CreateLogsAsync(long applicationId, LogLevel level, int counter = 1)
        {
            var application = await _applicationDao.GetAsync(applicationId);
            return await CreateLogsAsync(application, level, counter);
        }
    }
}