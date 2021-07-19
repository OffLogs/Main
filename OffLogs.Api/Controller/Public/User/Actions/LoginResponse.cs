using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Controller.Public.User.Actions
{
    public class LoginResponse: IResponse
    {
        public string Token { get; set; }
    }
}