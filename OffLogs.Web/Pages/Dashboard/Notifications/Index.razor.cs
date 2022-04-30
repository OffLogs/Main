using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Business.Common.Extensions;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui.NavigationLayout;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Notification;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Pages.Dashboard.Notifications;

public partial class Index
{
    [Inject] 
    private IState<NotificationRuleState> _state { get; set; }

    private NavigationLayout _navigationLayout { get; set; }
    
    private ICollection<HeaderMenuButton> _actionButtons = new List<HeaderMenuButton>();
    
    private bool _isShowAddRuleModal = false;
    
    private bool _isShowAddTemplateModal = false;
    
    private static readonly MenuItem _menuItemRules = new()
    {
        Id = "rules",
        Title = NotificationResources.MenuItem_Rules,
        Icon = "expand"
    };
    
    private static readonly MenuItem _menuItemTemplates = new()
    {
        Id = "templates", 
        Title = NotificationResources.MenuItem_MessageTemplates, 
        Icon = "layers"
    };
    
    private static ICollection<MenuItem> _menuItems { get; set; } = new List<MenuItem>()
    {
        _menuItemRules,
        _menuItemTemplates,
    };

    private MenuItem _selectedMenuItem = _menuItemRules;
    private ListItem _selectedListItem = null;
    private long _selectedListItemId = 0;

    private ICollection<ListItem> _listItems
    {
        get
        {
            if (_selectedMenuItem == _menuItemRules)
            {
                return _state.Value.Rules.Select(item => new ListItem
                {
                    Id = item.Id.ToString(),
                    Title = item.Id.ToString()
                }).ToList();
            }
            if (_selectedMenuItem == _menuItemTemplates)
            {
                return _state.Value.MessageTemplates.Select(item => new ListItem
                {
                    Id = item.Id.ToString(),
                    Title = item.Subject.Truncate(50),
                    SubTitle = item.Body.Truncate(50),
                }).ToList();
            }

            return new List<ListItem>();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        SetMainMenu();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            SetSelectedMenuItem(_menuItemRules);
        }
        return Task.CompletedTask;
    }
    
    private void OnClickAddButton()
    {
        _selectedListItemId = 0;
        _isShowAddRuleModal = _selectedMenuItem == _menuItemRules;
        _isShowAddTemplateModal = _selectedMenuItem == _menuItemTemplates;
    }
    
    private void OnClickEditButton()
    {
        OnClickAddButton();
        _selectedListItemId = _selectedListItem?.GetIdAsLong() ?? 0;
    }
    
    private void OnClickDeleteButton()
    {
        throw new NotImplementedException();
    }

    private void OnSelectMenuItem(OnSelectEventArgs menuEvent)
    {
        SetSelectedMenuItem(menuEvent.MenuItem);
    }

    private void OnSelectListItem(OnSelectEventArgs menuEvent)
    {
        _selectedListItem = menuEvent.ListItem;
        SetMainMenu();
    }

    private void OnCloseModal()
    {
        _isShowAddTemplateModal = _isShowAddRuleModal = false;
    }

    private void SetSelectedMenuItem(MenuItem item)
    {
        _selectedListItem = null;
        _selectedMenuItem = item;
        if (_selectedMenuItem == _menuItemRules)
        {
            Dispatcher.Dispatch(new FetchNotificationRulesAction());
        }
        if (_selectedMenuItem == _menuItemTemplates)
        {
            Dispatcher.Dispatch(new FetchMessageTemplatesAction());
        }
        
        _navigationLayout.SelectItem(_selectedMenuItem, false);
        SetMainMenu();
    }
    
    private void SetMainMenu()
    {
        var addMenuTitle = "";
        var editMenuTitle = "";
        if (_selectedMenuItem == _menuItemRules)
        {
            addMenuTitle = NotificationResources.MenuItem_AddRule;
            editMenuTitle = NotificationResources.MenuItem_EditRule;
        }
        if (_selectedMenuItem == _menuItemTemplates)
        {
            addMenuTitle = NotificationResources.MenuItem_AddTemplate;
            editMenuTitle = NotificationResources.MenuItem_EditTemplate;
        }
        
        _actionButtons.Clear();
        _actionButtons.Add(new HeaderMenuButton(addMenuTitle, "plus-square", OnClickAddButton));
        _actionButtons.Add(new HeaderMenuButton(editMenuTitle, "pencil-alt-2", OnClickEditButton)
        {
            IsDisabled = _selectedListItem == null
        });
        _actionButtons.Add(new HeaderMenuButton(NotificationResources.MenuItem_Delete, "basket", OnClickDeleteButton)
        {
            IsDisabled = _selectedListItem == null
        });
    }
}
