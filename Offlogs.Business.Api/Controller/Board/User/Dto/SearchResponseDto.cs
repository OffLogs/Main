using System.Collections.Generic;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto.Entities;

namespace Offlogs.Business.Api.Controller.Board.User.Dto
{
    public class SearchResponseDto: List<UserDto>, IResponse
    {
    }
}
