using Dapper;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Constants;
using OffLogs.Business.Constants.Dapper;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using JwtAuthService = OffLogs.Business.Services.Jwt.JwtAuthService;

namespace OffLogs.Business.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection InitCommonServices(this IServiceCollection services)
        {
            InitDbMappers();
            
            // System
            services.AddHttpContextAccessor();
            services.AddScoped<IDataFactoryService, DataFactoryService>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();
            services.AddScoped<IJwtApplicationService, JwtApplicationService>();
            
            // DAO
            services.AddScoped<ICommonDao, CommonDao>();
            services.AddScoped<IUserDao, UserDao>();
            services.AddScoped<IApplicationDao, ApplicationDao>();
            services.AddScoped<ILogDao, LogDao>();
            return services;
        }
        
        private static void InitDbMappers()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            // Extend Dapper types
            SqlMapper.AddTypeHandler(new DapperConstantHandler<CityCode>());
            SqlMapper.AddTypeHandler(new DapperConstantHandler<LogLevel>());
        }
    }
}