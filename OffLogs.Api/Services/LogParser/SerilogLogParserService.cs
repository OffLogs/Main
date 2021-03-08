using System;
using System.Linq;
using System.Threading.Tasks;
using OffLogs.Api.Models.Request.Log.Serilog;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Db.Entity;

namespace OffLogs.Api.Services.LogParser
{
    public class SerilogLogParserService: ISerilogLogParserService
    {
        private readonly ILogDao _logDao;
        
        public SerilogLogParserService(ILogDao logDao)
        {
            _logDao = logDao;
        }

        public async Task SaveAsync(long applicationId, SerilogEventsRequestModel model)
        {
            foreach (var log in model.Events)
            {
                var properties = log.Properties.Select(
                    property => new LogPropertyEntity(property.Key, property.Value)
                ).ToArray();
                var traces = log.Exception?.Split("\n").Select(
                    trace => new LogTraceEntity(trace)
                ).ToArray();
                await _logDao.AddAsync(
                    applicationId,
                    log.RenderedMessage,
                    log.LogLevel,
                    log.Timestamp,
                    properties,
                    traces
                );    
            }
        }
    }
}