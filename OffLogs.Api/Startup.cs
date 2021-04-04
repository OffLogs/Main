using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OffLogs.Api.Middleware;
using OffLogs.Business.Extensions;
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
            EnableSwaggerIntegration(services);
            services.InitCommonServices();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        // Get an instance of ILogger (see below) and log accordingly.
                        var body = context.HttpContext.Request.ReadBodyAsync().Result;
                        Log.Logger.Error($"Request data: {body}");
                        foreach (var value in context.ModelState.Values)
                        {
                            foreach (var error in value.Errors)
                            {
                                var errorMessage = !string.IsNullOrEmpty(error.ErrorMessage)
                                    ? error.ErrorMessage
                                    : error.Exception?.Message;
                                Log.Logger.Error(errorMessage);
                            }
                        }
                        return new BadRequestObjectResult(context.ModelState);
                    };
                })
                .AddNewtonsoftJson(options =>
                {
                    // Remove nullable fields from response Json
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddJsonOptions(options => {
                    // Ignore Null values in response models
                    // options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            var jwtSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    Configuration.GetValue<string>("App:Auth:SymmetricSecurityKey")
                )
            );
            services.AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,   
                        ValidIssuer = Configuration.GetValue<string>("App:Auth:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("App:Auth:Audience"), 
                        IssuerSigningKey = jwtSecurityKey,
                        ValidateLifetime = false,
                        ClockSkew = System.TimeSpan.FromMinutes(30000)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (_isRequestResponseLoggingEnabled)
            {
                app.UseMiddleware<RequestResponseLoggerMiddleware>();
            }
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private void EnableSwaggerIntegration(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OffLogs API",
                });
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }
                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);
                c.CustomSchemaIds(x => x.FullName);
            });
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
        }
    }
}