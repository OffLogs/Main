using System;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<ApplicationsListState> ApplicationsState { get; set; }
    
    private long? _selectedApplicationId;
    private ApplicationListItemDto _applicationForDeletion = null;
    private ApplicationListItemDto _applicationForSharing = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadListAsync(false);
    }

    private Task LoadListAsync(bool isLoadNextPage = true)
    {
        if (!isLoadNextPage)
        {
            Dispatcher.Dispatch(new ResetListAction());
        }
        Dispatcher.Dispatch(new FetchNextListPageAction());
        return Task.CompletedTask;
    }

    private async Task OnClickRowAsync(ApplicationListItemDto application)
    {
        _selectedApplicationId = application.Id;
        await Task.CompletedTask;
    }

    private void OnCloseInfoModal()
    {
        _selectedApplicationId = null;
    }

    private Task OnApplicationAdded(ApplicationDto application)
    {
        Dispatcher.Dispatch(new AddApplicationAction(new ApplicationListItemDto
        {
            Id = application.Id,
            CreateTime = application.CreateTime,
            Name = application.Name,
            UserId = application.UserId
        }));
        return Task.CompletedTask;
    }

    private async Task OnDeleteAppAsync()
    {
        _applicationForDeletion = _applicationForDeletion 
                                  ?? throw new ArgumentNullException(nameof(_applicationForDeletion));
        var applicationId = _applicationForDeletion.Id;
        _applicationForDeletion = null;
        Dispatcher.Dispatch(new DeleteApplicationAction(applicationId));
        await Task.CompletedTask;
    }
}
