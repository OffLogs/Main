using System.Collections.Generic;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log
{
    public class LogStatisticForNowDto : List<LogStatisticForNowItemDto>, IResponse
    {
        public LogStatisticForNowDto(ICollection<LogStatisticForNowItemDto> items)
        {
            AddRange(items);
        }
    }
}
