using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OffLogs.Api.Di.Autofac.Modules;
using OffLogs.Api.Extensions;
using OffLogs.Api.Middleware;
using OffLogs.Business;
using Serilog;

namespace OffLogs.Api
{
    public class Startup
    {
        private readonly bool _isRequestResponseLoggingEnabled;
        
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _isRequestResponseLoggingEnabled = configuration.GetValue("App:EnableRequestResponseLogging", false);
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddAutoMapper(typeof(ApiAssemblyMarker).Assembly);
            services.InitControllers();
            services.InitAuthServices(Configuration);
            services.InitSwaggerServices();
        }

        public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterModule<ApiModule>()
                .RegisterAssemblyModules(typeof(BusinessAssemblyMarker).Assembly);
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSerilogRequestLogging();
            }

            if (_isRequestResponseLoggingEnabled)
            {
                app.UseMiddleware<RequestResponseLoggerMiddleware>();
            }

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()
            );
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}