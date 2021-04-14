using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Web.Services;
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
            builder.Services.AddScoped(
                sp => new HttpClient
                {
                    // BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                    BaseAddress = new Uri(apiUrl)
                }
            );

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            
            await builder.Build().RunAsync();
        }
    }
}