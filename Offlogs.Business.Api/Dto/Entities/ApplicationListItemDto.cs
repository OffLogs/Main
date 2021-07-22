using System;

namespace Offlogs.Business.Api.Dto.Entities
{
    public class ApplicationListItemDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public string Name { get; set; }
        public DateTime CreateTime { get; set; }

        public ApplicationListItemDto()
        {
        }
    }
}
