using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;

namespace OffLogs.Api.Common.Requests.Board.Log
{
    public class LogStatisticForNowDto : List<LogStatisticForNowItemDto>, IResponse
    {
        public LogStatisticForNowDto(ICollection<LogStatisticForNowItemDto> items)
        {
            AddRange(items);
        }
    }
}