using Api.Requests.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Log
{
    public class GetBySharedTokenRequest: IRequest<LogSharedDto>
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Token { get; set; }
    }
}
