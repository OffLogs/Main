using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Application;
using OffLogs.Web.Core.Helpers;
using OffLogs.Web.Core.Utils;
using OffLogs.Web.Resources;
using OffLogs.Web.Services.Http;
using Radzen;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class ApplicationsDropDown
{
    [Parameter]
    public long Value
    {
        get => _selectedId;
        set => _selectedId = value;
    }
    
    [Parameter]
    public EventCallback<long> ValueChanged { get; set; }

    [Parameter]
    public EventCallback<ApplicationListItemDto> SelectedItemChanged { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; } = CommonResources.SelectApplication;
    
    [Parameter]
    public string Class { get; set; }
    
    [Inject]
    public ILogger<ApplicationsDropDown> _logger { get; set; }
    
    [Inject]
    public IApiService _apiService { get; set; }
    
    private ICollection<ApplicationListItemDto> _list = new List<ApplicationListItemDto>();

    private ICollection<ApplicationListItemDto> _listToShow = new List<ApplicationListItemDto>();

    private int _count = 0;
    
    private int _page = 0;
    
    private bool _hasMore = true;
    
    private int _skip = 0;
    
    private int _take = 0;

    private ApplicationListItemDto _selectedItem;

    private long _selectedId = 0;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadNextList();
    }

    private async Task LoadData(LoadDataArgs filter)
    {
        await LoadNextList();
        if (filter.Skip.HasValue)
        {
            _skip = filter.Skip.Value;
        }
        if (filter.Top.HasValue)
        {
            _take = filter.Top.Value;
        }
        _listToShow = _list.Skip(_skip).Take(_take).ToList();   
    }

    private async Task LoadNextList()
    {
        if (!_hasMore)
        {
            return;
        }

        _page += 1;
        var response = await _apiService.GetApplicationsAsync(new GetListRequest()
        {
            Page = _page
        });
        if (response == null)
        {
            return;
        }

        _hasMore = response.IsHasMore;
        _count = response.TotalCount;
        lock (_list)
        {
            _list = _list.Concat(response.Items).ToList();
        }
    }
    
    private Task OnValueChanged(long selectedId)
    {
        _selectedItem = _list.FirstOrDefault(item => item.Id == selectedId);
        SelectedItemChanged.InvokeAsync(_selectedItem);
        ValueChanged.InvokeAsync(selectedId);
        return Task.CompletedTask;
    }
}
