using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Notification.Effects;

public class FetchMessageTemplatesEffect: Effect<FetchNextListPageAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;
    private readonly ILogger<FetchMessageTemplatesEffect> _logger;

    public FetchMessageTemplatesEffect(
        IState<ApplicationsListState> state,
        IApiService apiService,
        ILogger<FetchMessageTemplatesEffect> logger
    )
    {
        _state = state;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchNextListPageAction pageAction, IDispatcher dispatcher)
    {
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
