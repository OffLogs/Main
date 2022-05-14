using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Statistic
{
    public class GetApplicationStatisticRequest : IRequest<ApplicationStatisticDto>
    {
        [IsPositive(AllowZero = true)]
        public long ApplicationId { get; set; }
    }
}
