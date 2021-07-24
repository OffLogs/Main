using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Requests.Board.Log;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log
{
    public class GetLogStatisticForNowRequest : IRequest<LogStatisticForNowDto>
    {
        [IsPositive]
        public long? ApplicationId { get; set; }
    }
}
