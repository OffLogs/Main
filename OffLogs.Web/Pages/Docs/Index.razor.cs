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
    [Parameter]
    public string? MainPart { get; set; }
    
    [Parameter]
    public string? SubPath { get; set; }
    
    [Inject]
    private IFilesCache _filesCacheService { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }

    private readonly string _baseUrl = "/files/docs/";

    private string _markdownString = "";
    
    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _navigationManager.LocationChanged += OnLocationChanged;
        await LoadBody();
    }

    private void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(async () => await LoadBody());
    }

    private async Task LoadBody()
    {
        if (string.IsNullOrEmpty(SubPath))
        {
            _markdownString = "### Not found";
            return;
        }

        var fileName = $"{_baseUrl}{SubPath}";
        _isLoading = true;
        _markdownString = await _filesCacheService.LoadAndCache(fileName, "md");
        _isLoading = false;
        StateHasChanged();
    }
}
