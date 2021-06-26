using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OffLogs.Api.Tests.Integration.Api.Main
{
    public class ApiTestStartup: Startup
    {
        public ApiTestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}