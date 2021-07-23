using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Business.Controller.Board.Application.Actions
{
    public class GetListRequest : IRequest<PaginatedListDto<ApplicationListItemDto>>
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}
