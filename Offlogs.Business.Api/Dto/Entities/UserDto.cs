using Api.Requests.Abstractions;

namespace Offlogs.Business.Api.Dto.Entities
{
    public class UserDto : IResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }
}
