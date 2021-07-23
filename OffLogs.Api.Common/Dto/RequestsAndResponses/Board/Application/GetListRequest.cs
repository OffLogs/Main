using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application
{
    public class GetListRequest : IRequest<PaginatedListDto<ApplicationListItemDto>>
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}
