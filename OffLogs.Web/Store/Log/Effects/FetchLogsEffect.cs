using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log;
using OffLogs.Web.Core.Utils;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Auth;
using OffLogs.Web.Store.Log.Models;

namespace OffLogs.Web.Store.Log.Effects;

public class FetchLogsEffect: Effect<FetchListPageAction>
{
    private readonly IState<LogsListState> _state;
    private readonly IState<AuthState> _authState;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchLogsEffect> _logger;

    public FetchLogsEffect(
        IState<LogsListState> state,
        IState<AuthState> authState,
        IApiService apiService,
        ILogger<FetchLogsEffect> logger
    )
    {
        _state = state;
        _authState = authState;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchListPageAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            var filterData = _state.Value.Filter ?? new LogFilterModel();
            if (filterData.ApplicationId == 0)
            {
                _logger.LogDebug("Application was not selected. Skip fetching logs");
                dispatcher.Dispatch(new ResetListAction());
                return;
            }

            var page = PaginationUtils.CalculatePage(_state.Value.SkipItems, _state.Value.PageSize);
            var response = await _apiService.GetLogsAsync(new GetListRequest
            {
                Page = page,
                ApplicationId = filterData.ApplicationId,
                LogLevel = filterData.LogLevel,
                IsFavorite = filterData.IsOnlyFavorite,
                CreateTimeFrom = filterData.CreateTimeFrom,
                CreateTimeTo = filterData.CreateTimeTo,
                PrivateKeyBase64 = _authState.Value.PrivateKeyBase64
            });
            dispatcher.Dispatch(new FetchListResultAction(response));
            dispatcher.Dispatch(new UpdateFilteredItemsAction());
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
