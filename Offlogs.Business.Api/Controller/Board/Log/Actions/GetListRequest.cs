using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using Offlogs.Business.Api.Dto;
using Offlogs.Business.Api.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Log.Actions
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
