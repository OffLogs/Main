using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Store.Application.Effects;

public class FetchApplicationsEffect: Effect<FetchListPageAction>
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

    public override async Task HandleAsync(FetchListPageAction pageAction, IDispatcher dispatcher)
    {   
        try
        {
            Debug.Log(pageAction.Skip, _state.Value.PageSize, pageAction.Skip / _state.Value.PageSize);
            var page = (int) Math.Ceiling((decimal) (pageAction.Skip / _state.Value.PageSize));
            page = page == 0 ? 1 : page + 1;
            var response = await _apiService.GetApplicationsAsync(new GetListRequest
            {
                Page = page
            });
            dispatcher.Dispatch(new FetchListResultAction(
                response.Items,
                response.TotalPages,
                response.PageSize,
                response.TotalCount,
                response.IsHasMore
            ));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
