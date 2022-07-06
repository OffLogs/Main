using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using NHibernate.Util;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Store.UserEmails.Effects;

public class FetchListEffect: Effect<FetchListAction>
{
    private readonly IApiService _apiService;
    private readonly ILogger<FetchListEffect> _logger;
    private readonly IState<UserEmailsState> _state;

    public FetchListEffect(
        IApiService apiService,
        ILogger<FetchListEffect> logger,
        IState<UserEmailsState> state
    )
    {
        _apiService = apiService;
        _logger = logger;
        _state = state;
    }

    public override async Task HandleAsync(FetchListAction pageAction, IDispatcher dispatcher)
    {   
        try
        {
            if (pageAction.IsLoadIfEmpty && _state.Value.List.Any())
            {
                dispatcher.Dispatch(new SetIsLoadingAction(false));
                return;
            }

            var response = await _apiService.UserEmailsGetList();
            dispatcher.Dispatch(new FetchListResultAction(response));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
