using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Business;
using OffLogs.Business.Extensions;
using OffLogs.WorkerService.LogDeletion;
using OffLogs.WorkerService.LogProcessing;

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
            //services.AddScoped<ILogProcessingService, LogProcessingService>();
            //services.AddHostedService<LogProcessingHostedService>();

            // Log deletion
            services.AddScoped<ILogDeletionService, LogDeletionService>();
            services.AddHostedService<LogDeletionHostedService>();
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