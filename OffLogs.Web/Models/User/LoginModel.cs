using System.ComponentModel.DataAnnotations;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Public.User;

namespace OffLogs.Web.Models.User;

public class LoginModel: LoginRequest
{
    [Required]
    [StringLength(32, MinimumLength = 6)]
    public string Password { get; set; }

    public LoginRequest Request =>
        new()
        {
            SignBase64 = SignBase64,
            SignedData = SignedData,
            PublicKeyBase64 = PublicKeyBase64
        };
}
