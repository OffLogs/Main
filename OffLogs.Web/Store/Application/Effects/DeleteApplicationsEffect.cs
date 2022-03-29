using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Resources;
using OffLogs.Web.Services;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Store.Application.Effects;

public class DeleteApplicationsEffect: Effect<DeleteApplicationAction>
{
    private readonly IState<ApplicationsListState> _state;
    private readonly IApiService _apiService;
    private readonly ILogger<DeleteApplicationAction> _logger;
    private readonly ToastService _toastService;

    public DeleteApplicationsEffect(
        IState<ApplicationsListState> state,
        IApiService apiService,
        ILogger<DeleteApplicationAction> logger,
        ToastService toastService
    )
    {
        _state = state;
        _apiService = apiService;
        _logger = logger;
        _toastService = toastService;
    }

    public override async Task HandleAsync(DeleteApplicationAction pageAction, IDispatcher dispatcher)
    {
        try
        {
            await _apiService.DeleteApplicationAsync(pageAction.Id);
            dispatcher.Dispatch(new RemoveApplicationFromListAction(pageAction.Id));
            _toastService.AddInfoMessage(ApplicationResources.ApplicationIsDeleted);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            _toastService.AddErrorMessage(ApplicationResources.ApplicationDeletionError);
        }
    }
}
