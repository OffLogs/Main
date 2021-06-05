using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using OffLogs.Business.Db.Entity;
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
            using (var session = Session)
            {
                return await session.Query<ApplicationEntity>().Where(
                    application => application.Id == applicationId && application.User.Id == userId
                ).AnyAsync();
            }
        }
        
        public async Task<(ICollection<ApplicationEntity>, long)> GetList(long userId, int page, int pageSize = 30)
        {
            page = page - 1;
            var offset = (page <= 0 ? 0 : page) * pageSize;

            var sumCounter = await Connection.CountAsync<ApplicationEntity>(entity => entity.UserId == userId);
            var listQuery = Connection.From<ApplicationEntity>()
                .Where<ApplicationEntity>(log => log.UserId == userId)
                .Limit(offset, pageSize)
                .OrderBy<LogEntity>(log => log.CreateTime);
            var list = await Connection.SelectAsync(listQuery);
            return (list, sumCounter);
        }
    }
}