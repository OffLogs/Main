using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OffLogs.Api.Tests.Integration
{
    public class TestStartup: Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
        
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }
    }
}