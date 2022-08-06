using Autofac;
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
                .RegisterType<UserInfoProducerRedisClient>()
                .As<IUserInfoProducerRedisClient>()
                .InstancePerLifetimeScope();
            
            builder
                .RegisterType<UserInfoConsumerRedisClient>()
                .As<IUserInfoConsumerRedisClient>()
                .InstancePerLifetimeScope();
        }
    }
}
