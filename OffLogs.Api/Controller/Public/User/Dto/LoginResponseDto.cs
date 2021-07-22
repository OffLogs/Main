using Api.Requests.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Dto
{
    public class LoginResponseDto: IResponse
    {
        public string Token { get; set; }
    }
}