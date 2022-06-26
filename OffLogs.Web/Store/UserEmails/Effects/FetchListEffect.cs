using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Store.UserEmails.Effects;

public class FetchListEffect: Effect<FetchListAction>
{
    private readonly IApiService _apiService;
    private readonly ILogger<FetchListEffect> _logger;

    public FetchListEffect(
        IApiService apiService,
        ILogger<FetchListEffect> logger
    )
    {
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchListAction pageAction, IDispatcher dispatcher)
    {   
        try
        {
            var response = await _apiService.UserEmailsGetList();
            dispatcher.Dispatch(new FetchListResultAction(response));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
