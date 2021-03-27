using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Services.Jwt;
using ServiceStack.OrmLite;

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
            await Connection.InsertAsync(application);
            application.ApiToken = _jwtService.BuildJwt(application.Id);
            await Connection.UpdateAsync(application);
            return application;
        }
    }
}