using System;
using System.Threading.Tasks;
using Fluxor;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application.Effects;

public class FetchApplicationsEffect: Effect<FetchApplicationListAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;

    public FetchApplicationsEffect(
        IState<ApplicationsListState> state,
        IApiService apiService
    )
    {
        _state = state;
        _apiService = apiService;
    }

    public override async Task HandleAsync(FetchApplicationListAction action, IDispatcher dispatcher)
    {
        try
        {
            var applications = await _apiService.GetApplicationsAsync(new GetListRequest
            {
                Page = _state.Value.Page
            });
        }
        catch (Exception e)
        {
            // TODO: Add toast loading error
        }
    }
}
