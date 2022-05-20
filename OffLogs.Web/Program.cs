using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Majorsoft.Blazor.WebAssembly.Logging.Console;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OffLogs.Business.Common.Services.Web;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Events;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Services.i18;
using OffLogs.Web.Services.IO;
using OffLogs.Web.Services.Validation;
using Radzen;

namespace OffLogs.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var currentAssembly = typeof(Program).Assembly;
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            InitServices(builder);
            
            // Store
            builder.Services.AddFluxor(
                options =>
                {
                    options.ScanAssemblies(currentAssembly);
#if DEBUG
                    options.UseReduxDevTools();
#endif
                }
            );
            
#if DEBUG
            // Init logger
            builder.Logging.AddBrowserConsole()
                .SetMinimumLevel(LogLevel.Debug) //Setting LogLevel is optional
                .AddFilter("Microsoft", LogLevel.Information); //System logs can be filtered.
#endif
            
            var host = builder.Build();
            
            // Set localization
            var localizationService = host.Services.GetRequiredService<ILocalizationService>();
            await localizationService.SetUpLocaleAsync();
            
            await host.RunAsync();
        }

        private static void InitServices(WebAssemblyHostBuilder builder)
        {
            var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");
            // System services
            builder.Services.AddScoped(
                sp => new HttpClient
                {
                    BaseAddress = new Uri(apiUrl)
                }
            );
            builder.Services.AddBlazoredLocalStorage();

            // Radzen services
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();
            
            // Custom services
            builder.Services.AddLocalization();
            builder.Services.AddScoped<ILocalizationService, LocalizationService>();
            builder.Services.AddScoped<IGlobalEventsService, GlobalEventsService>();
            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>();
            builder.Services.AddScoped<IFilesCache, FilesCache>();
            builder.Services.AddScoped<IMarkdownService, MarkdownService>();
            builder.Services.AddScoped<ToastService>();
        }
    }
}
