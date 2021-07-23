using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Home
{
    public record PingRequest : IRequest<PongResponse>
    {
    }
}
