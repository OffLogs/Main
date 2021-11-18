using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class CheckIsLoggedInRequestHandle : IAsyncRequestHandler<CheckIsLoggedInRequest>
    {
        public Task ExecuteAsync(CheckIsLoggedInRequest request)
        {
            return Task.CompletedTask;
        }
    }
}