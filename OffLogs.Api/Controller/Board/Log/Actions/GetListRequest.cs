using Api.Requests.Abstractions;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Log.Actions
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
