using Api.Requests.Abstractions;
using OffLogs.Api.Dto.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.User.Dto
{
    public class SearchResponseDto: List<UserDto>, IResponse
    {
    }
}
