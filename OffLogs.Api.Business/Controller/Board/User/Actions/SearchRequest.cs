using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Controller.Board.User.Dto;

namespace OffLogs.Api.Business.Controller.Board.User.Actions
{
    public class SearchRequest: IRequest<SearchResponseDto>
    {
        [StringLength(100, MinimumLength = 2)]
        public string Search { get; set; }
    }
}
