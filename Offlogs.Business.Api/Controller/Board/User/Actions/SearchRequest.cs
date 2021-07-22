using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Controller.Board.User.Dto;

namespace Offlogs.Business.Api.Controller.Board.User.Actions
{
    public class SearchRequest: IRequest<SearchResponseDto>
    {
        [StringLength(100, MinimumLength = 2)]
        public string Search { get; set; }
    }
}
