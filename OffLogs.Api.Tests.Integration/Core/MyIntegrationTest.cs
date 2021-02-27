using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OffLogs.Api.Tests.Integration;
using OffLogs.Business.Db.Dao;
using OffLogs.Business.Services;
using Xunit;

namespace Vizit.Api.Mobile.Tests.Integration.Core
{
    public class MyIntegrationTest:IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _factory;

        protected ICommonDao Dao;
        protected readonly IJwtService jwtService;
        
        public MyIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            var configuration = _factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            jwtService = _factory.Services.GetService(typeof(IJwtService)) as IJwtService;
        }

        public async Task<HttpResponseMessage> PostRequestAsAnonymousAsync(string url, object data = null)
        {
            var client = _factory.CreateClient();
            var requestData = JsonContent.Create(data ?? new { });
            return await client.PostAsync(url, requestData);
        }
        
        public async Task<HttpResponseMessage> PostRequestAsync(string url, string jwtToken,  object data = null)
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var requestData = JsonContent.Create(data ?? new {});
            return await client.PostAsync(url, requestData);
        }
        
        // protected (MobileProfileEntity, String) CreateMobileProfileWithToken()
        // {   
        //     var mobileProfile = CommonDataSeeder.CreateMobileProfile();
        //     var token = jwtService.BuildJwt(mobileProfile.Id);
        //     return (mobileProfile, token);
        // }
    }
}