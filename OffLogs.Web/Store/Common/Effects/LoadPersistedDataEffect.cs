using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Store.Common.Effects;

public class LoadPersistedDataAEffect: AEffectPersistData<LoadPersistedDataAction>
{
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<LoadPersistedDataAEffect> _logger;
    private readonly IState<CommonState> _state;

    public LoadPersistedDataAEffect(
        ILocalStorageService localStorage,
        ILogger<LoadPersistedDataAEffect> logger,
        IState<CommonState> state
    )
    {
        _localStorage = localStorage;
        _logger = logger;
        _state = state;
    }

    public override async Task HandleAsync(LoadPersistedDataAction pageAction, IDispatcher dispatcher)
    {
        if (_state.Value.IsInitialized)
        {
            return;
        }

        _logger.LogDebug("Load persisted data from local storage");
        var authData = await GetData<AuthState>(AuthDataKey);
        if (authData != null && !string.IsNullOrEmpty(authData.Pem))
        {
            dispatcher.Dispatch(new LoginAction(authData));
        }
        dispatcher.Dispatch(new SetIsAppInitializedAction());
    }

    private async Task<TState> GetData<TState>(string key)
    {
        try
        {
            var authDataString = await _localStorage.GetItemAsStringAsync(key);
            if (string.IsNullOrEmpty(authDataString))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<TState>(authDataString);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }

        return default;
    }
}
