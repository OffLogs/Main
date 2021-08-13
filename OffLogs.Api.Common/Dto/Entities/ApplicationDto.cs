using System;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Dto;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class ApplicationDto : IResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string ApiToken { get; set; }
        public DateTime CreateTime { get; set; }
        public PermissionInfoDto Permissions { get; set; }

        public ApplicationDto()
        {
            Permissions = new PermissionInfoDto(false, false);
        }
    }
}
