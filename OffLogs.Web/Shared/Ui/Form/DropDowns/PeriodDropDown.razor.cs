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
    public int Value
    {
        get => _value;
        set => _value = value;
    }
    
    [Parameter]
    public EventCallback<int> ValueChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = CommonResources.SelectItem;
    
    [Parameter]
    public string Class { get; set; }
    
    private int _value;

    private Dictionary<int, string> _list = new()
    {
        { 300, "5 minutes" },
        { 900, "15 minutes" },
        { 1_800, "30 minutes" },
        { 3_600, "1 hour" },
        { 10_800, "3 hours" },
        { 21_600, "6 hours" },
        { 43_200, "12 hours" },
        { 86_400, "1 day" },
        { 259_200, "3 days" },
        { 604_800, "weekly" },
    };
    
    private void OnItemSelected(KeyValuePair<int, string> item)
    {
        _value = item.Key;
        ValueChanged.InvokeAsync(_value);
        StateHasChanged();
    }
}
