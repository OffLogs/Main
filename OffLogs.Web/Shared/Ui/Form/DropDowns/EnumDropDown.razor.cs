using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Web.Resources;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class EnumDropDown<TItem>
{
    [Parameter]
    public TItem Value
    {
        get => _value;
        set => _value = value;
    }
    
    [Parameter]
    public EventCallback<TItem> ValueChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = CommonResources.SelectItem;
    
    [Parameter]
    public string Class { get; set; }
    
    private List<TItem> _list;
    private TItem _value;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _list = Enum.GetValues(typeof(TItem)).Cast<TItem>().ToList();
    }
    
    private void OnItemSelected(TItem level)
    {
        _value = level;
        ValueChanged.InvokeAsync(_value);
    }
}
