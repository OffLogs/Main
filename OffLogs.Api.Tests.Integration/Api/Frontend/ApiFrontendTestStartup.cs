using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OffLogs.Api.Tests.Integration.Api.Frontend
{
    public class ApiFrontendTestStartup: OffLogs.Api.Frontend.Startup
    {
        public ApiFrontendTestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}