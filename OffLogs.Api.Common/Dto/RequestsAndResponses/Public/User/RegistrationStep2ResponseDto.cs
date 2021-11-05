using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class RegistrationStep2ResponseDto : IResponse
    {
        public string Pem { get; set; }
    }
}