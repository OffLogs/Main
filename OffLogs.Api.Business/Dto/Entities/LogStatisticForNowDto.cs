using System.Collections.Generic;
using Api.Requests.Abstractions;

namespace OffLogs.Api.Business.Dto.Entities
{
    public class LogStatisticForNowDto: List<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto>, IResponse
    {
        public LogStatisticForNowDto(ICollection<OffLogs.Business.Orm.Dto.Entities.LogStatisticForNowDto> items)
        {
            AddRange(items);
        }
    }
}