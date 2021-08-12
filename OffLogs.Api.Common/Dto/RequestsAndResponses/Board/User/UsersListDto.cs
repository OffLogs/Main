using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User
{
    public class UsersListDto : List<UserDto>, IResponse
    {
    }
}
