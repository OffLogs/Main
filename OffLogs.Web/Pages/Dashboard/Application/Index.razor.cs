using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Application;
using Radzen;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<ApplicationsListState> State { get; set; }

    private bool _isShowAddModal = false;
    
    private bool _isShowDeleteModal = false;

    private ICollection<HeaderMenuButton> _buttons = new List<HeaderMenuButton>();

    private ICollection<MenuItem> _menuItems
    {
        get
        {
            return State.Value.List.Select(
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
        _buttons.Add(
            new HeaderMenuButton(ApplicationResources.DeleteApplication, "basket", OnDeleteApplicationClick)
        );
        
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

    private void OnDeleteApplicationClick()
    {
        if (!State.Value.SelectedApplicationId.HasValue)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = ApplicationResources.SelectApplication
            });
            return;
        }
        _isShowDeleteModal = true;
    }
    
    private async Task OnDeleteAppAsync()
    {
        Dispatcher.Dispatch(new DeleteApplicationAction(
            State.Value.SelectedApplicationId.Value    
        ));
        _isShowDeleteModal = false;
        await Task.CompletedTask;
    }
    
    private void OnApplicationSelected(OnSelectEventArgs menuEvent)
    {
        Dispatcher.Dispatch(new SelectApplicationAction(
            long.Parse(menuEvent.MenuItem.Id)
        ));
    }
    
    private void OnCloseAddModal()
    {
        _isShowAddModal = false;
    }
}
