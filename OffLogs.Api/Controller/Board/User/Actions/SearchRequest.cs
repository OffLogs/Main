using Api.Requests.Abstractions;
using OffLogs.Api.Controller.Board.User.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.User.Actions
{
    public class SearchRequest: IRequest<SearchResponseDto>
    {
        [StringLength(100, MinimumLength = 2)]
        public string Search { get; set; }
    }
}
