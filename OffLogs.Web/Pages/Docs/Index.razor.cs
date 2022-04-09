using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.i18;
using OffLogs.Web.Services.IO;
using OffLogs.Web.Shared.Ui.NavigationLayout.Models;

namespace OffLogs.Web.Pages.Docs;

public partial class Index
{
    [Inject]
    private IFilesCache _filesCacheService { get; set; }

    private readonly string _baseUrl = "/files/docs/";
    private readonly ICollection<MenuItem> _menuItems = new List<MenuItem>();
    private ICollection<ListItem> _documentsList = new List<ListItem>();

    private MarkupString? _body = new("");
    public bool _isLoading = false;

    private readonly MenuItem _menuItemCommon = new() {Id = "common", Title = DocResources.MenuItem_Common};
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _menuItems.Add(_menuItemCommon);
        SetDocumentsList(_menuItems.First());
    }

    private void OnMenuItemSelected(OnSelectEventArgs item)
    {
        SetDocumentsList(item.MenuItem);
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
                }
            };
        }
        else
        {
            _documentsList = new List<ListItem>();
        }
    }

    private async Task OnSelectPageAsync(OnSelectEventArgs menuEvent)
    {
        var fileName = $"{_baseUrl}{menuEvent.ListItem.Id}";
        await LoadBody(fileName);
    }

    private async Task<bool> LoadBody(string filePath)
    {
        _isLoading = true;
        var fileContent = await _filesCacheService.LoadAndCache(filePath, "md");

        var htmlString = "";
        var result = false;
        if (fileContent != null)
        {
            result = true;
            htmlString = MarkdownHelper.ToHtml(fileContent);
        }
        _body = new MarkupString(htmlString);

        _isLoading = false;
        return result;
    }
}
