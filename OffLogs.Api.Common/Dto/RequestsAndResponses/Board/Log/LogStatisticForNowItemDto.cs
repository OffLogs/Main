using OffLogs.Business.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log
{
    public class LogStatisticForNowItemDto
    {
        public long Count { get; set; }

        public LogLevel LogLevel { get; set; }

        public DateTime TimeInterval { get; set; }
    }
}
