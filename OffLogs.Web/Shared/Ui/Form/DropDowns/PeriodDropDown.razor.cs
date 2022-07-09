using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Resources;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class PeriodDropDown
{
    [Parameter]
    public TimeSpan Value
    {
        get => _value;
        set => _value = value;
    }
    
    [Parameter]
    public EventCallback<TimeSpan> ValueChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = CommonResources.SelectItem;
    
    [Parameter]
    public string Class { get; set; }
    
    [Parameter]
    public bool IsAllowClear { get; set; }
    
    private TimeSpan _value;

    private List<TimeSpan> _list = new()
    {
        TimeSpan.FromMinutes(5),
        TimeSpan.FromMinutes(15),
        TimeSpan.FromMinutes(30),
        TimeSpan.FromHours(1),
        TimeSpan.FromHours(3),
        TimeSpan.FromHours(6),
        TimeSpan.FromHours(12),
        TimeSpan.FromDays(1),
        TimeSpan.FromDays(3),
        TimeSpan.FromDays(7)
    };
    
    private void OnItemSelected(TimeSpan item)
    {
        _value = item;
        ValueChanged.InvokeAsync(_value);
        StateHasChanged();
    }
}
