using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OffLogs.Api.Middleware;
using OffLogs.Business.Extensions;
using Serilog;

namespace OffLogs.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public virtual void ConfigureServices(IServiceCollection services)
        {
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

            app.UseMiddleware<RequestResponseLoggerMiddleware>();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}