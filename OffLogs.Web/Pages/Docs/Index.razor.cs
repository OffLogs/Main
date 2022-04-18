using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using OffLogs.Web.Constants;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.IO;
using OffLogs.Web.Shared.Ui.NavigationLayout;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Docs;

public partial class Index: IDisposable
{
    [Inject]
    private IFilesCache _filesCacheService { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Parameter]
    public string? MenuItemId { get; set; }
    
    [Parameter]
    public string? ListItemId { get; set; }

    private NavigationLayout _navigationLayout { get; set; }
    
    private readonly string _baseUrl = "/files/docs/";
    private readonly ICollection<MenuItem> _menuItems = new List<MenuItem>();
    private ICollection<ListItem> _documentsList = new List<ListItem>();

    private string _markdownString = "";
    private bool _isLoading = false;

    private readonly MenuItem _menuItemCommon = new() {Id = "common", Title = DocResources.MenuItem_Common};
    private readonly MenuItem _menuItemApi = new() {Id = "api", Title = DocResources.MenuItem_Api};
    private readonly MenuItem _menuItemMessageResource = new() {Id = "message_resource", Title = DocResources.MenuItem_MessageResource};
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _menuItems.Add(_menuItemCommon);
        _menuItems.Add(_menuItemApi);
        _menuItems.Add(_menuItemMessageResource);
        SetDocumentsList(_menuItems.First());

        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        InvokeAsync(async () =>
        {
            await SetSelectedItemsAsync();
        });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InvokeAsync(async () =>
            {
                await SetSelectedItemsAsync();
            });
        }
    }
    
    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
    
    private async Task SetSelectedItemsAsync()
    {
        var menuItem = _menuItems.FirstOrDefault(item => item.Id == MenuItemId?.ToLower());
        if (menuItem == null)
        {
            return;
        }
        SetDocumentsList(menuItem);
        _navigationLayout.SelectItem(menuItem, false);
        var sumMenuItem = _documentsList.FirstOrDefault(item => item.Id == ListItemId?.ToLower());
        if (sumMenuItem != null)
        {
            _navigationLayout.SelectItem(sumMenuItem, false);
            await LoadBody(sumMenuItem);
        }
    }

    private void OnMenuItemSelected(OnSelectEventArgs item)
    {
        SetDocumentsList(item.MenuItem);
        _navigationManager.NavigateTo($"{SiteUrl.Documentation}/{item.MenuItem.Id}");
    }

    private void SetDocumentsList(MenuItem menuItem)
    {
        if (menuItem == _menuItemCommon)
        {
            _documentsList = new List<ListItem>()
            {
                new()
                {
                    Id = "1_1_common",
                    Title = "Common Info"
                },
                new()
                {
                    Id = "1_2_how_it_works",
                    Title = "How it works?"
                },
                new()
                {
                    Id = "1_3_applications",
                    Title = "Common Info"
                },
            };
        }
        else if (menuItem == _menuItemApi)
        {
            _documentsList = new List<ListItem>()
            {
                new()
                {
                    Id = "2_1_rest_api",
                    Title = DocResources.DocumentTitle_RestApi
                },
                new()
                {
                    Id = "2_2_dot_net",
                    Title = DocResources.DocumentTitle_DotNet
                },
                new()
                {
                    Id = "2_3_net_core_extension",
                    Title = DocResources.DocumentTitle_DotNetCore
                },
                new()
                {
                    Id = "2_4_serilog_extension",
                    Title = DocResources.DocumentTitle_SerilogExtension
                },
            };
        }
        else if (menuItem == _menuItemMessageResource)
        {
            _documentsList = new List<ListItem>()
            {
                new()
                {
                    Id = "3_1_log_level",
                    Title = DocResources.DocumentTitle_LogLevel
                },
            };
        }
        else
        {
            _documentsList = new List<ListItem>();
        }
    }

    private async Task OnSelectPageAsync(OnSelectEventArgs menuEvent)
    {
        await LoadBody(menuEvent.ListItem);
        _navigationManager.NavigateTo($"{SiteUrl.Documentation}/{menuEvent.MenuItem.Id}/{menuEvent.ListItem.Id}");
    }

    private async Task LoadBody(ListItem listItem)
    {
        var fileName = $"{_baseUrl}{listItem.Id}";
        _isLoading = true;
        _markdownString = await _filesCacheService.LoadAndCache(fileName, "md");
        _isLoading = false;
        StateHasChanged();
    }
}
