using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Multi;
using NHibernate.Proxy;
using OffLogs.Business.Constants;
using OffLogs.Business.Db.Entity;
using LogLevel = OffLogs.Business.Constants.LogLevel;

namespace OffLogs.Business.Db.Dao
{
    public class RequestLogDao: CommonDao, IRequestLogDao
    {
        public RequestLogDao(IConfiguration configuration, ILogger<CommonDao> logger) : base(configuration, logger)
        {
        }

        public async Task<RequestLogEntity> AddAsync(RequestLogType type, string clientIp, object data, string token = null)
        {
            return await AddAsync(type, clientIp, JsonConvert.SerializeObject(data), token);
        }

        public async Task<RequestLogEntity> GetByTokenAsync(string token)
        {
            using (var session = Session)
            {
                return await session.Query<RequestLogEntity>()
                    .Where(e => e.Token == token)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<RequestLogEntity> AddAsync(RequestLogType type, string clientIp, string data, string token = null)
        {
            using (var session = Session)
            using(var transaction = session.BeginTransaction())
            {
                // Clear bags
                var log = new RequestLogEntity
                {
                    Type = type,
                    ClientIp = clientIp,
                    Data = data,
                    Token = token,
                    CreateTime = DateTime.Now
                };
                log.Id = (long)await session.SaveAsync(log);
                await transaction.CommitAsync();
                return log;
            }
        }
    }
}