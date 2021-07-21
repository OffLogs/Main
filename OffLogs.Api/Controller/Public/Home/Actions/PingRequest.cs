using Api.Requests.Abstractions;

namespace OffLogs.Api.Controller.Public.Home.Actions
{
    public record PingRequest: IRequest<PongResponse>
    {
    }
}
