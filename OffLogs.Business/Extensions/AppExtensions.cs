using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Constants;
using OffLogs.Business.Constants.Dapper;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services;
using OffLogs.Business.Services.Jwt;

namespace OffLogs.Business.Extensions
{
    public static class AppExtensions
    {
        public static void InitCommonServices(this IServiceCollection services)
        {
            InitDbMappers();
            
            services.AddHttpContextAccessor();
            services.AddScoped<IJwtAuthService, JwtAuthService>();
            services.AddScoped<IJwtApplicationService, JwtApplicationService>();
            services.AddScoped<ICommonDao, CommonDao>();
            services.AddScoped<IUserDao, UserDao>();
        }
        
        private static void InitDbMappers()
        {
            // Extend Dapper types
            SqlMapper.AddTypeHandler(new DapperConstantHandler<CityCode>());
            SqlMapper.AddTypeHandler(new DapperConstantHandler<LogLevel>());
        }
    }
}