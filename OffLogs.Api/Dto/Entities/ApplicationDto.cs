using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Dto.Entities
{
    public class ApplicationDto
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
