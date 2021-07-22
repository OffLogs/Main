using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Application.Actions
{
    public class GetListRequest : IRequest<PaginatedListDto<ApplicationListItemDto>>
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}
