using Api.Requests.Abstractions;
using Autofac;
using Microsoft.AspNetCore.Http;
using OffLogs.Business.Services.Api;
using OffLogs.Business.Services.Kafka;

namespace OffLogs.Api.Frontend.Di.Autofac.Modules
{
    public class KafkaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<KafkaProducerService>()
                .As<IKafkaProducerService>()
                .SingleInstance();
        }
    }
}