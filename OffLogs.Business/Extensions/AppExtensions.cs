using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Constants;
using OffLogs.Business.Constants.Dapper;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services;

namespace OffLogs.Business.Extensions
{
    public static class AppExtensions
    {
        public static void InitCommonServices(this IServiceCollection services)
        {
            InitDbMappers();
            
            services.AddHttpContextAccessor();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICommonDao, CommonDao>();
        }
        
        private static void InitDbMappers()
        {
            // Extend Dapper types
            SqlMapper.AddTypeHandler(new DapperConstantHandler<CityCode>());
            SqlMapper.AddTypeHandler(new DapperConstantHandler<LogLevel>());
        }
    }
}