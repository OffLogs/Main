using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.UserEmail
{
    public class GetListRequest : IRequest<UsersListDto>
    {
        [StringLength(100, MinimumLength = 2)]
        public string Search { get; set; }
    }
}
