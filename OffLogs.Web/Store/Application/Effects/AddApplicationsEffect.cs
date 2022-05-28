using System;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Services.Http;

namespace OffLogs.Web.Store.Application.Effects;

public class AddApplicationsEffect: Effect<AddApplicationAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;
    private readonly ILogger<AddApplicationsEffect> _logger;

    public AddApplicationsEffect(
        IState<ApplicationsListState> state,
        IApiService apiService,
        ILogger<AddApplicationsEffect> logger
    )
    {
        _state = state;
        _apiService = apiService;
        _logger = logger;
    }

    public override async Task HandleAsync(AddApplicationAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            var application = await _apiService.AddApplicationAsync(pageAction.Name);
            dispatcher.Dispatch(new AddApplicationListItemAction(application));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}
