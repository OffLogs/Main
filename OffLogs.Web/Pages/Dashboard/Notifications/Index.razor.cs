using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Resources;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;
using OffLogs.Web.Store.Notification;
using OffLogs.Web.Store.Notification.Actions;

namespace OffLogs.Web.Pages.Dashboard.Notifications;

public partial class Index
{
    [Inject] 
    private IState<NotificationRuleState> _state { get; set; }

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
                    Title = item.Id.ToString()
                }).ToList();
            }

            return new List<ListItem>();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _actionButtons.Add(
            new HeaderMenuButton(ApplicationResources.AddApplication, "plus-square", OnClickAddButton)
        );
    }

    private void OnClickAddButton()
    {
        _isShowAddRuleModal = _selectedMenuItem == _menuItemRules;
        _isShowAddTemplateModal = _selectedMenuItem == _menuItemTemplates;
    }

    private void OnSelectMenuItem(OnSelectEventArgs menuEvent)
    {
        _selectedMenuItem = menuEvent.MenuItem;
        if (_selectedMenuItem == _menuItemRules)
        {
            Dispatcher.Dispatch(new FetchNotificationRulesAction());
        }
        if (_selectedMenuItem == _menuItemTemplates)
        {
            Dispatcher.Dispatch(new FetchMessageTemplatesAction());
        }
    }
    
    private void OnCloseModal()
    {
        _isShowAddTemplateModal = _isShowAddRuleModal = false;
    }
}
