using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.User
{
    public class SearchRequest : IRequest<SearchResponseDto>
    {
        [StringLength(100, MinimumLength = 2)]
        public string Search { get; set; }
    }
}
