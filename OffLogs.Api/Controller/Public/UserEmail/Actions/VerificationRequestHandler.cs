using System.Threading.Tasks;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.UserEmail;
using OffLogs.Business.Services.Entities.UserEmail;

namespace OffLogs.Api.Controller.Public.UserEmail.Actions
{
    public class VerificationRequestHandler : IAsyncRequestHandler<VerificationRequest>
    {
        private readonly IUserEmailService _userEmailService;

        public VerificationRequestHandler(
            IUserEmailService userEmailService
        )
        {
            _userEmailService = userEmailService;
        }

        public async Task ExecuteAsync(VerificationRequest request)
        {
            await _userEmailService.VerifyByToken(request.Token);
        }
    }
}
