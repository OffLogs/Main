using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Business.Dto;
using OffLogs.Api.Business.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class GetListRequest: IRequest<PaginatedListDto<LogListItemDto>>
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }

        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }

        public LogLevel? LogLevel { get; set; }
    }
}
