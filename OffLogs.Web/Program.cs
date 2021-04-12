using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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

            builder.Services.AddScoped(
                sp => new HttpClient
                {
                    // BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                    BaseAddress = new Uri("https://api.offlogs.com")
                }
            );

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            
            await builder.Build().RunAsync();
        }
    }
}