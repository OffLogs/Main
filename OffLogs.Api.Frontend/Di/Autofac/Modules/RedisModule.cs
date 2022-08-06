using Api.Requests.Abstractions;
using Autofac;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Kafka;
using OffLogs.Business.Services.Redis;
using OffLogs.Business.Services.Redis.Clients;

namespace OffLogs.Api.Frontend.Di.Autofac.Modules
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RedisClient>()
                .As<IRedisClient>()
                .SingleInstance();            
            
            builder
                .RegisterType<UserInfoRedisClient>()
                .As<IUserInfoRedisClient>()
                .InstancePerLifetimeScope();
        }
    }
}
