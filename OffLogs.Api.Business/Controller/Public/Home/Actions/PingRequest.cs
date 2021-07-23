using Api.Requests.Abstractions;

namespace OffLogs.Api.Business.Controller.Public.Home.Actions
{
    public record PingRequest: IRequest<PongResponse>
    {
    }
}
