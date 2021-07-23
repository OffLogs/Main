using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commands.Abstractions;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Orm.Commands.Context;
using OffLogs.Business.Orm.Entities;
using OffLogs.Business.Orm.Queries;
using OffLogs.Business.Orm.Queries.Entities.Log;
using OffLogs.Business.Services.Jwt;
using Queries.Abstractions;

namespace OffLogs.Business.Services.Entities.Log
{
    public class LogService: ILogService
    {
        private readonly IAsyncCommandBuilder _commandBuilder;
        private readonly IAsyncQueryBuilder _queryBuilder;
        private readonly IJwtApplicationService _jwtService;

        public LogService(
            IAsyncCommandBuilder commandBuilder,
            IAsyncQueryBuilder queryBuilder,
            IJwtApplicationService jwtService
        )
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
            _jwtService = jwtService;
        }
        
        public async Task<LogEntity> AddAsync(LogEntity log)
        {
            // Clear bags
            var properties = log.Properties.ToList();
            log.Properties = new List<LogPropertyEntity>();
            var traces = log.Traces.ToList();
            log.Traces = new List<LogTraceEntity>();
                
            await _commandBuilder.SaveAsync(log);

            properties.ForEach(log.AddProperty);
            traces.ForEach(log.AddTrace);
            return log;
        }
        
        public async Task<LogEntity> AddAsync(
            ApplicationEntity application,  
            string message,
            LogLevel level,
            DateTime timestamp,
            ICollection<LogPropertyEntity> properties = null,
            ICollection<LogTraceEntity> traces = null
        )
        {
            var log = new LogEntity()
            {
                Application = application,
                Message = message,
                Level = level,
                LogTime = timestamp
            };
            if (properties != null)
            {
                foreach (var logPropertyEntity in properties)
                {
                    log.AddProperty(logPropertyEntity);
                }
            }
            if (traces != null)
            {
                foreach (var logTraceEntity in traces)
                {
                    log.AddTrace(logTraceEntity);
                }
            }
            await _commandBuilder.SaveAsync(log);
            return log;
        }
        
        public async Task<bool> SetIsFavoriteAsync(long userId, long logId, bool isFavorite)
        {
            var log = await _queryBuilder.FindByIdAsync<LogEntity>(logId);
            if (log == null)
                throw new ArgumentNullException(nameof(log));
            var user = await _queryBuilder.FindByIdAsync<UserEntity>(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var isAlreadyFavorite = await _queryBuilder.For<bool>().WithAsync(
                new LogIsFavoriteCriteria(userId, logId)
            );
            if (isAlreadyFavorite)
            {
                return false;
            }
            user.FavoriteLogs.Add(log);
            await _commandBuilder.SaveAsync(user);
            return true;
        }
    }
}