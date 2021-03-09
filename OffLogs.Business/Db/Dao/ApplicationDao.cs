using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Db.Entity;
using OffLogs.Business.Helpers;
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
        
        public async Task<bool> IsOwner(long userId, long applicationId)
        {
            var inputParams = new
            {
                UserId = userId, 
                ApplicationId = applicationId
            };
            var parameters = new DynamicParameters(inputParams);
            using var multi = await Connection.QueryMultipleAsync(
                "SELECT dbo.fn_ApplicationIsOwner(@UserId, @ApplicationId)",
                parameters
            );
            return multi.Read<bool>().First();
        }
    }
}