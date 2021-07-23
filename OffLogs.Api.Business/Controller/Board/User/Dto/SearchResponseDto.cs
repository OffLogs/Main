using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto.Entities;

namespace OffLogs.Api.Business.Controller.Board.User.Dto
{
    public class SearchResponseDto: List<UserDto>, IResponse
    {
    }
}
