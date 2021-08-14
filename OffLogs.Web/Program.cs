using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Majorsoft.Blazor.WebAssembly.Logging.Console;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Events;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");
            
            // System services
            builder.Services.AddScoped(
                sp => new HttpClient
                {
                    BaseAddress = new Uri(apiUrl)
                }
            );
            builder.Services.AddBlazoredLocalStorage();

            // Custom services
            builder.Services.AddScoped<IGlobalEventsService, GlobalEventsService>();
            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<ToastService>();
            
            // Init logger
            builder.Logging.AddBrowserConsole()
                .SetMinimumLevel(LogLevel.Debug) //Setting LogLevel is optional
                .AddFilter("Microsoft", LogLevel.Information); //System logs can be filtered.

            await builder.Build().RunAsync();
        }
    }
}