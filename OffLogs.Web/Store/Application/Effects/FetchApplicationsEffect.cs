using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application.Effects;

public class FetchApplicationsEffect: Effect<FetchNextListPageAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<FetchApplicationsEffect> _logger;

    public FetchApplicationsEffect(
        IState<ApplicationsListState> state,
        IApiService apiService,
        IDispatcher dispatcher,
        ILogger<FetchApplicationsEffect> logger
    )
    {
        _state = state;
        _apiService = apiService;
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public override async Task HandleAsync(FetchNextListPageAction pageAction, IDispatcher dispatcher)
    {
        PaginatedListDto<ApplicationListItemDto> response = null;
        try
        {
            response = await _apiService.GetApplicationsAsync(new GetListRequest
            {
                Page = _state.Value.Page
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
        finally
        {
            _dispatcher.Dispatch(new FetchListResultAction(response));    
        }
    }
}
