using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class UserEmailDto : IResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        
        public bool IsVerified { get; set; }
    }
}
