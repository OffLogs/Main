using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User
{
    public class LoginResponseDto : IResponse
    {
        public string Token { get; set; }
        
        public string PrivateKeyBase64 { get; set; }
    }
}
