using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application
{
    public class GetSharedUsersRequest : IRequest<UsersListDto>
    {
        [Required]
        public long Id { get; set; }
    }
}
