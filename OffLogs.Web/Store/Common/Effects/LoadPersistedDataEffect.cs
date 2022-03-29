using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fluxor;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Auth.Actions;
using OffLogs.Web.Store.Common.Actions;

namespace OffLogs.Web.Store.Common.Effects;

public class LoadPersistedDataAEffect: AEffectPersistData<LoadPersistedDataAction>
{
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<LoadPersistedDataAEffect> _logger;

    public LoadPersistedDataAEffect(
        ILocalStorageService localStorage,
        ILogger<LoadPersistedDataAEffect> logger
    )
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    public override async Task HandleAsync(LoadPersistedDataAction pageAction, IDispatcher dispatcher)
    {
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
