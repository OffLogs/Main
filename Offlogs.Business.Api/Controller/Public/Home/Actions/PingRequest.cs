using Api.Requests.Abstractions;

namespace Offlogs.Business.Api.Controller.Public.Home.Actions
{
    public record PingRequest: IRequest<PongResponse>
    {
    }
}
