using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Application;
using OffLogs.Web.Store.Application.Actions;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<ApplicationsListState> ApplicationsState { get; set; }

    private bool _isShowAddModal = false;
    
    private ApplicationListItemDto _applicationForDeletion = null;
    private ApplicationListItemDto _applicationForSharing = null;

    private ICollection<HeaderMenuButton> _buttons = new List<HeaderMenuButton>();

    private ICollection<MenuItem> _menuItems
    {
        get
        {
            return ApplicationsState.Value.Applications.Select(
                application => new MenuItem()
                {
                    Id = application.Id.ToString(),
                    Title = application.Name
                }
            ).ToList();
        }
    }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _buttons.Add(
            new HeaderMenuButton(ApplicationResources.AddApplication, "plus-square", () => _isShowAddModal = true)
        );
        
        ApplicationsState.StateChanged += OnStateChanged;
        
        await LoadListAsync(false);
    }

    private void OnStateChanged(object? sender, EventArgs e)
    {
        
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
        await Task.CompletedTask;
    }

    private void OnCloseInfoModal()
    {
        Dispatcher.Dispatch(new SelectApplicationAction(null));
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
    
    private void OnApplicationSelected(OnSelectEventArgs menuEvent)
    {
        Debug.Log($"111", menuEvent);
        Dispatcher.Dispatch(new SelectApplicationAction(
            long.Parse(menuEvent.MenuItem.Id)
        ));
    }
}
