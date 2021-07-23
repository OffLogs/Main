using Api.Requests.Abstractions;

namespace OffLogs.Api.Business.Controller.Public.User.Dto
{
    public class LoginResponseDto: IResponse
    {
        public string Token { get; set; }
    }
}