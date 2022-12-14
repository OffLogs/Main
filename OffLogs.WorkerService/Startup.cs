using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Business;

namespace OffLogs.WorkerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<Services.LogsProcessingHostedService>();
            services.AddHostedService<Services.LogsDeletionHostedService>();
            services.AddHostedService<Services.NotificationProcessingHostedService>();
            services.AddHostedService<Services.NotificationRuleProcessingHostedService>();
            services.AddHostedService<Services.Redis.UserInfoSeedingHostedService>();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyModules(typeof(BusinessAssemblyMarker).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
