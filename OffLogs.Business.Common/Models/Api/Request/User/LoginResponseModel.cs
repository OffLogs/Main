namespace OffLogs.Business.Common.Models.Api.Request.User
{
    public record LoginResponseModel
    {
        public string Token { get; set; }

        public LoginResponseModel() {}
    }
}