﻿using Api.Requests.Abstractions;

namespace OffLogs.Api.Controller.Public.Home.Actions
{
    public record PongResponse: IResponse
    {
        public bool Pong { get; set; } = true;
    }
}
