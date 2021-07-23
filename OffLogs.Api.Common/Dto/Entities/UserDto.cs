using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class UserDto : IResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }
}
