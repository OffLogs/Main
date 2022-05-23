using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OffLogs.Business.Common.Constants;
using OffLogs.Web.Resources;

namespace OffLogs.Web.Shared.Ui.Form.DropDowns;

public partial class LogLevelDropDown
{
    [Parameter]
    public LogLevel? Value
    {
        get => _value;
        set => _value = value ?? default;
    }
    
    [Parameter]
    public EventCallback<LogLevel?> ValueChanged { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = CommonResources.LogLevel;
    
    private List<LogLevel> _list;
    private LogLevel _value;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _list = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
    }
    
    private void OnItemSelected(LogLevel level)
    {
        _value = level;
        ValueChanged.InvokeAsync(_value);
    }
}
