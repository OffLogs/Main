using Api.Requests.Abstractions;

namespace Offlogs.Business.Api.Controller.Public.Home.Actions
{
    public record PongResponse: IResponse
    {
        public bool Pong { get; set; } = true;
    }
}
