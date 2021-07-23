using System;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Business.Dto.Entities
{
    public class ApplicationDto: IResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public string Name { get; set; }
        public string ApiToken { get; set; }
        public DateTime CreateTime { get; set; }

        public ApplicationDto()
        {
        }
    }
}
