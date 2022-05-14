using OffLogs.Business.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Orm.Dto.Entities
{
    public class ApplicationStatisticDto
    {
        public long LogsCount { get; set; }
        
        public long ErrorLogsCount { get; set; }
        
        public long WarningLogsCount { get; set; }
        
        public long FatalLogsCount { get; set; }
        
        public long InformationLogsCount { get; set; }
        
        public long DebugLogsCount { get; set; }
        
        public long TracesCount { get; set; }
        
        public long PropertiesCount { get; set; }
    }
}
