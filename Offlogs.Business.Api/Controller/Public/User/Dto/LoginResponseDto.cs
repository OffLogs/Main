using Api.Requests.Abstractions;

namespace Offlogs.Business.Api.Controller.Public.User.Dto
{
    public class LoginResponseDto: IResponse
    {
        public string Token { get; set; }
    }
}