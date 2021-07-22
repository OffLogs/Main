﻿using System.Threading.Tasks;
using Api.Requests.Abstractions;

namespace Offlogs.Business.Api.Controller.Public.User.Actions
{
    public class CheckIsLoggedInRequestHandle : IAsyncRequestHandler<CheckIsLoggedInRequest>
    {
        public Task ExecuteAsync(CheckIsLoggedInRequest request)
        {
            return Task.CompletedTask;
        }
    }
}