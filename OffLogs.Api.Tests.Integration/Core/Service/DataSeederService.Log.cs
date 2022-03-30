using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;

namespace OffLogs.Api.Tests.Integration.Core.Service
{
    public partial class DataSeederService
    {

        public async Task<LogEntity> MakeLogAsync(ApplicationEntity application, LogLevel level)
        {
            var logFactory = _dataFactory.LogFactory(level);
            var log = logFactory.Generate();
            log.Application = application;
                
            var logTraceFactory = _dataFactory.LogTraceFactory();
            logTraceFactory.GenerateLazy(4)
                .ToList()
                .ForEach(log.AddTrace);
            var logPropertyFactory = _dataFactory.LogPropertyFactory();
            logPropertyFactory.GenerateLazy(3)
                .ToList()
                .ForEach(log.AddProperty);
            return await Task.FromResult(log);
        }
        
        public async Task<List<LogEntity>> CreateLogsAsync(ApplicationEntity application, LogLevel level, int counter = 1)
        {
            var result = new List<LogEntity>();
            for (int i = 1; i <= counter; i++)
            {
                var log = await MakeLogAsync(application, level);

                var propertiesDictionary = new Dictionary<string, object>();
                foreach (var item in log.Properties)
                {
                    propertiesDictionary.Add(item.Key, item.Value);
                }

                log = await _logService.AddAsync(
                    log.Application,
                    log.Message,
                    log.Level,
                    log.LogTime,
                    propertiesDictionary,
                    log.Traces.Select(item => item.Trace).ToList()
                );
                result.Add(log);
            }
            return result;
        }

        public async Task<List<LogEntity>> CreateLogsAsync(long applicationId, LogLevel level, int counter = 1)
        {
            var application = await _queryBuilder.FindByIdAsync<ApplicationEntity>(applicationId);
            return await CreateLogsAsync(application, level, counter);
        }
    }
}
