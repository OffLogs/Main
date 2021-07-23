using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Public.Home
{
    public record PongResponse : IResponse
    {
        public bool Pong { get; set; } = true;
    }
}
