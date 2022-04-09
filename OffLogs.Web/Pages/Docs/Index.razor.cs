﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Resources;
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
        _documentsList.Add(
            new ListItem
            {
                Id = "1_1_common",
                Title = "Item1",
                SubTitle = "ItemsSub"
            }
        );
    }

    private void OnMenuItemSelected(OnSelectEventArgs item)
    {
        if (item.MenuItem == _menuItemCommon)
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

        throw new System.NotImplementedException();
    }

    private async Task OnSelectPageAsync(OnSelectEventArgs menuEvent)
    {
        await LoadBody($"{_baseUrl}{menuEvent.ListItem.Id}.md");
    }

    private async Task LoadBody(string filePath)
    {
        _isLoading = true;
        var fileContent = await _filesCacheService.LoadAndCache(filePath);

        var htmlString = "";
        if (fileContent != null)
        {
            htmlString = MarkdownHelper.ToHtml(fileContent);
        }
        _body = new MarkupString(htmlString);

        _isLoading = false;
    }
}
