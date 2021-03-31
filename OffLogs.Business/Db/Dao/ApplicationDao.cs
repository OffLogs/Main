using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Jwt;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

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

        public async Task<ApplicationEntity> CreateNewApplication(long userId,  string name)
        {   
            var application = new ApplicationEntity()
            {
                UserId = userId,
                Name = name,
                ApiToken = "tempToken",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            application.Id = await Connection.InsertAsync(application, selectIdentity: true);
            application.ApiToken = _jwtService.BuildJwt(application.Id);
            await Connection.UpdateAsync(application);
            return application;
        }
        
        public async Task<bool> IsOwner(long userId, long applicationId)
        {
            var inputParams = new
            {
                UserId = userId, 
                ApplicationId = applicationId
            };
            var isExists = await Connection.ExistsAsync<ApplicationEntity>(
                application => application.Id == applicationId && application.UserId == userId
            );
            return isExists;
        }
    }
}