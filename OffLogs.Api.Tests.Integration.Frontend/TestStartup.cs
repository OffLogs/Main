using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Api.Frontend;

namespace OffLogs.Api.Tests.Integration.Frontend
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