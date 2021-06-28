using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services.Data;
using OffLogs.Business.Services.Jwt;
using OffLogs.Business.Services.Kafka;
using JwtAuthService = OffLogs.Business.Services.Jwt.JwtAuthService;

namespace OffLogs.Business.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection InitAllServices(this IServiceCollection services)
        {
            services.InitBaseServices();
            services.InitDbServices();
            return services;
        }
        
        public static IServiceCollection InitDbServices(this IServiceCollection services)
        {
            // DAO
            services.AddScoped<ICommonDao, CommonDao>();
            services.AddScoped<IUserDao, UserDao>();
            services.AddScoped<IApplicationDao, ApplicationDao>();
            services.AddScoped<ILogDao, LogDao>();
            services.AddScoped<IRequestLogDao, RequestLogDao>();
            return services;
        }
        
        public static IServiceCollection InitBaseServices(this IServiceCollection services)
        {
            // System
            services.AddHttpContextAccessor();
            services.AddScoped<IDataFactoryService, DataFactoryService>();
            services.AddScoped<IJwtAuthService, JwtAuthService>();
            services.AddScoped<IJwtApplicationService, JwtApplicationService>();
            
            // Kafka
            services.AddSingleton<IKafkaProducerService, KafkaProducerProducerService>();
            services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
            return services;
        }
    }
}