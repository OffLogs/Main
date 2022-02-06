using OffLogs.Business.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Dto.Entities
{
    public class LogStatisticForNowDto
    {
        public long Count { get; set; }
        
        public long LevelId { get; set; }

        public LogLevel LogLevel {
            get {
                Enum.TryParse<LogLevel>(LevelId.ToString(), out var parsedValue);
                return parsedValue;
            }
        }

        public DateTimeOffset TimeInterval { get; set; }
    }
}
