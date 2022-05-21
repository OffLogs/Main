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
using Radzen;
using Radzen.Blazor;

namespace OffLogs.Web.Pages.Dashboard.Application;

public partial class Index
{
    [Inject]
    private IApiService ApiService { get; set; }

    [Inject]
    private IState<ApplicationsListState> State { get; set; }

    private bool _isShowAddModal = false;
    
    private bool _isShowDeleteModal = false;
    
    RadzenDataGrid<ApplicationListItemDto> _grid;

    private ICollection<HeaderMenuButton> _buttons = new List<HeaderMenuButton>();

    private ApplicationListItemDto _applicationToInsert = null;
    
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
    
    private async Task UpdateApplication(ApplicationListItemDto app)
    {
        try
        {
            var application = await ApiService.UpdateApplicationAsync(app.Id, app.Name);
            Dispatcher.Dispatch(new UpdateApplicationAction(application));
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = ApplicationResources.ApplicationWasUpdated
            });
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = e.Message
            });
        }
    }
    
    private async Task AddApplication(ApplicationListItemDto app)
    {
        try
        {
            var application = await ApiService.AddApplicationAsync(app.Name);
            Dispatcher.Dispatch(new AddApplicationAction(new ApplicationListItemDto
            {
                Id = application.Id,
                CreateTime = application.CreateTime,
                Name = application.Name,
                UserId = application.UserId
            }));
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = "New application was added"
            });
        }
        catch (Exception e)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = e.Message
            });
        }
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

    async Task InsertRow()
    {
        _applicationToInsert = new ApplicationListItemDto();
        await _grid.InsertRow(_applicationToInsert);
    }
    
    async Task EditRow(ApplicationListItemDto app)
    {
        await _grid.EditRow(app);
    }

    async Task SaveRow(ApplicationListItemDto app)
    {
        await _grid.UpdateRow(app);
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
            await AddApplication(app);
        }
        else
        {
            await UpdateApplication(app);
        }
    }

    void CancelEdit(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
        }
        _grid.CancelEditRow(app);
    }

    private async Task DeleteRow(ApplicationListItemDto app)
    {
        if (app == _applicationToInsert)
        {
            _applicationToInsert = null;
        }
        
        var isOk = await DialogService.Confirm(
            ApplicationResources.DeleteConfirmation,
            CommonResources.DeletionConfirmation,
            new ConfirmOptions()
            {
                OkButtonText = "Ok",
                CancelButtonText = CommonResources.Cancel
            }
        );
        if (isOk.HasValue && isOk.Value)
        {
            Dispatcher.Dispatch(new DeleteApplicationAction(
                app.Id
            ));
        }
    }
}
