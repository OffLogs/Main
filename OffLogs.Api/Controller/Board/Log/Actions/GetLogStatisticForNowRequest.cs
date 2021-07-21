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
    public class GetLogStatisticForNowRequest : IRequest<LogStatisticForNowDto>
    {
        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }
    }
}
