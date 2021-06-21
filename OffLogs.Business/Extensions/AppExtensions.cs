using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Communication;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using JwtAuthService = OffLogs.Business.Services.Jwt.JwtAuthService;

namespace OffLogs.Business.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection InitCommonServices(this IServiceCollection services)
        {
            // System
            services.AddHttpContextAccessor();
            services.AddScoped<IDataFactoryService, DataFactoryService>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();
            services.AddScoped<IJwtApplicationService, JwtApplicationService>();
            services.AddScoped<IKafkaService, KafkaService>();
            
            // DAO
            services.AddScoped<ICommonDao, CommonDao>();
            services.AddScoped<IUserDao, UserDao>();
            services.AddScoped<IApplicationDao, ApplicationDao>();
            services.AddScoped<ILogDao, LogDao>();
            return services;
        }
    }
}