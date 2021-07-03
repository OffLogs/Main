using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Multi;
using OffLogs.Business.Db.Entities;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Business.Db.Dao
{
    public class ApplicationDao: CommonDao, IApplicationDao
    {
        private readonly IJwtApplicationService _jwtService;
        
        public ApplicationDao(
            IConfiguration configuration, 
            ILogger<ApplicationDao> logger,
            IJwtApplicationService jwtService
        ) : base(
            configuration,
            logger
        )
        {
            this._jwtService = jwtService;
        }

        public async Task<ApplicationEntity> GetAsync(long applicationId)
        {
            using var session = Session;
            return await session.GetAsync<ApplicationEntity>(applicationId);
        }

        public async Task<ApplicationEntity> CreateNewApplication(long userId,  string name)
        {   
            using var session = Session;
            using var transaction = session.BeginTransaction();
            var user = session.Load<UserEntity>(userId);
            return await CreateNewApplication(user, name);
        }
        
        public async Task<ApplicationEntity> CreateNewApplication(UserEntity user,  string name)
        {   
            var application = new ApplicationEntity()
            {
                User = user,
                Name = name,
                ApiToken = "tempToken",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            using (var session = Session)
            using (var transaction = session.BeginTransaction())
            {
                
                application.Id = (long)await session.SaveAsync(application);
                application.ApiToken = _jwtService.BuildJwt(application.Id);
                await session.UpdateAsync(application);
                await transaction.CommitAsync();
                return application;
            }
        }
        
        public async Task<ApplicationEntity> UpdateApplication(long applicationId, string name)
        {
            var application = await GetOneAsync<ApplicationEntity>(applicationId);
            using (var session = Session)
            using (var transaction = session.BeginTransaction())
            {
                application.Name = name;
                application.UpdateTime = DateTime.Now;
                await session.UpdateAsync(application);
                await transaction.CommitAsync();
                return application;
            }   
        }
        
        public async Task<bool> IsOwner(long userId, long applicationId)
        {
            using var session = Session;
            return await session.Query<ApplicationEntity>().Where(
                application => application.Id == applicationId && application.User.Id == userId
            ).AnyAsync();
        }
        
        public async Task<(ICollection<ApplicationEntity>, long)> GetList(long userId, int page, int pageSize = 30)
        {
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            using var session = Session;
            var query = session.Query<ApplicationEntity>()
                .Where(record => record.User.Id == userId);
            var listQuery = query
                .Skip(offset)
                .Take(pageSize)
                .OrderBy(log => log.CreateTime)
                .ToFuture();
            var count = await query.CountAsync();
            var list = await listQuery.GetEnumerableAsync();
            return (list.ToList(), count);
        }
    }
}