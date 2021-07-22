using Api.Requests.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Dto.Entities
{
    public class UserDto : IResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }
}
