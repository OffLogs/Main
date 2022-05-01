using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Store.Application.Effects;

public class FetchApplicationsEffect: Effect<FetchNextListPageAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchApplicationsEffect> _logger;

    public FetchApplicationsEffect(
        IState<ApplicationsListState> state,
        IApiService apiService,
        ILogger<FetchApplicationsEffect> logger
    )
    {
        _state = state;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchNextListPageAction pageAction, IDispatcher dispatcher)
    {
        if (pageAction.IsLoadIfEmpty && _state.Value.List.Any())
        {
            return;
        }
        
        try
        {
            var response = await _apiService.GetApplicationsAsync(new GetListRequest
            {
                Page = _state.Value.Page
            });
            dispatcher.Dispatch(new FetchListResultAction(
                response.Items,
                response.TotalPages,
                response.TotalCount,
                response.PageSize,
                response.IsHasMore
            ));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
